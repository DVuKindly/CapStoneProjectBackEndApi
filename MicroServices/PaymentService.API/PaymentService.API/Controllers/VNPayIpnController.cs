using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Data;
using PaymentService.API.Entities;
using PaymentService.API.Helpers;
using PaymentService.API.Services;
using SharedKernel.DTOsChung;
using System.Text.Json;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/payments/vnpay-ipn")]
    public class VNPayIpnController : ControllerBase
    {
        private readonly PaymentDbContext _db;
        private readonly IConfiguration _config;
        private readonly IPaymentResultHandler _resultHandler;

        public VNPayIpnController(
            PaymentDbContext db,
            IConfiguration config,
            IPaymentResultHandler resultHandler)
        {
            _db = db;
            _config = config;
            _resultHandler = resultHandler;
        }

        [HttpGet]
        public async Task<IActionResult> VNPayIpn()
        {
            var vnpay = new VnPayLibrary();

            // 1. Parse query string
            foreach (var key in Request.Query.Keys)
            {
                var value = Request.Query[key];
                if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value)
                    && key != "vnp_SecureHash" && key != "vnp_SecureHashType")
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            // 2. Get params
            string inputHash = Request.Query["vnp_SecureHash"];
            string txnRef = Request.Query["vnp_TxnRef"];
            string transactionNo = Request.Query["vnp_TransactionNo"];
            string responseCode = Request.Query["vnp_ResponseCode"];
            string transactionStatus = Request.Query["vnp_TransactionStatus"];
            string bankCode = Request.Query["vnp_BankCode"];

            // 3. Validate signature
            string secretKey = _config["VNPay:HashSecret"];
            bool isValidSignature = vnpay.ValidateSignature(inputHash, secretKey, encodeHash: false);

            if (!isValidSignature)
                return BadRequest("Sai chữ ký");

            // 4. Lookup payment request
            var payment = await _db.PaymentRequests.FirstOrDefaultAsync(p => p.RequestCode.ToLower() == txnRef.ToLower());
            if (payment == null)
                return NotFound("Không tìm thấy giao dịch");

            if (payment.Status == "Success" || payment.Status == "Paid")
                return Ok("Giao dịch đã xử lý");

            if (responseCode == "00" && transactionStatus == "00")
            {
                // 5. Mark as Paid
                payment.Status = "Paid";
                payment.PaidAt = DateTime.UtcNow;
                payment.UpdatedAt = DateTime.UtcNow;

                _db.PaymentTransactions.Add(new PaymentTransaction
                {
                    Id = Guid.NewGuid(),
                    PaymentRequestId = payment.Id,
                    TransactionId = transactionNo ?? Guid.NewGuid().ToString(),
                    Gateway = "vnpay",
                    Status = "Success",
                    Amount = payment.Amount,
                    PayDate = DateTime.UtcNow,
                    ConfirmedAt = DateTime.UtcNow,
                    BankCode = bankCode,
                    GatewayResponse = JsonSerializer.Serialize(vnpay.ResponseData),
                    CreatedAt = DateTime.UtcNow
                });

                await _db.SaveChangesAsync();

                // 6. Notify UserService
                if (payment.MembershipRequestId != Guid.Empty && payment.MembershipRequestId != null)
                {
                    var markPaidDto = new MarkPaidRequestDto
                    {
                        RequestId = payment.MembershipRequestId,
                        PaymentMethod = "VNPAY",
                        PaymentTransactionId = transactionNo ?? Guid.NewGuid().ToString(),
                        PaymentNote = "Auto updated via VNPay IPN",
                    };

                    try
                    {
                        await _resultHandler.HandleSuccessfulPaymentAsync(markPaidDto);
                        Console.WriteLine("✅ Notify UserService OK (IPN)");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Notify UserService failed (IPN): " + ex.Message);
                    }
                }

                return Ok("IPN xử lý thành công");
            }

            // 7. Mark as Failed
            payment.Status = "Failed";
            payment.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok("IPN cập nhật thất bại");
        }
    }
}
