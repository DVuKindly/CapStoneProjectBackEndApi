using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentService.API.Data;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.Entities;
using PaymentService.API.Helpers;
using PaymentService.API.Services;
using PaymentService.API.Services.Interfaces;
using SharedKernel.DTOsChung;
using System.Text.Json;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/payments/vnpayreturn")]
    public class VNPayReturnController : ControllerBase
    {
        private readonly PaymentDbContext _db;
        private readonly IConfiguration _config;
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IPaymentResultHandler _resultHandler;

        public VNPayReturnController(
            PaymentDbContext db,
            IConfiguration config,
            IHttpContextHelper httpContextHelper,
            IPaymentResultHandler resultHandler)
        {
            _db = db;
            _config = config;
            _httpContextHelper = httpContextHelper;
            _resultHandler = resultHandler;
        }

        [HttpGet]
        public async Task<IActionResult> VNPayReturn()
        {
            var vnpay = new VnPayLibrary();

            // 1. Parse query params
            foreach (var key in Request.Query.Keys)
            {
                var value = Request.Query[key];
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value)
                    && key != "vnp_SecureHash" && key != "vnp_SecureHashType")
                {
                    vnpay.AddResponseData(key, value);
                }
            }

            // 2. Get required params
            string inputHash = Request.Query["vnp_SecureHash"];
            string txnRef = Request.Query["vnp_TxnRef"].FirstOrDefault() ?? Request.Query["ref"].FirstOrDefault();
            string transactionNo = Request.Query["vnp_TransactionNo"].FirstOrDefault() ?? Request.Query["transactionNo"].FirstOrDefault();
            string responseCode = Request.Query["vnp_ResponseCode"].FirstOrDefault() ?? Request.Query["code"].FirstOrDefault();
            string transactionStatus = Request.Query["vnp_TransactionStatus"].FirstOrDefault() ?? Request.Query["status"].FirstOrDefault();
            string bankCode = Request.Query["vnp_BankCode"];

            if (string.IsNullOrWhiteSpace(txnRef))
                return BadRequest("Thiếu mã giao dịch");

            // 3. Verify signature
            string secretKey = _config["VNPay:HashSecret"];
            bool skipSignatureValidation = Request.Host.Host.Contains("localhost") || Request.Host.Host.Contains("ngrok");
            bool isValidSignature = skipSignatureValidation || vnpay.ValidateSignature(inputHash, secretKey, encodeHash: false);

            if (!isValidSignature)
                return BadRequest("Sai chữ ký VNPay");

            // 4. Get PaymentRequest
            var payment = await _db.PaymentRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.RequestCode.ToLower() == txnRef.ToLower());

            if (payment == null)
                return NotFound("Không tìm thấy giao dịch");

            var paymentToUpdate = await _db.PaymentRequests.FirstAsync(p => p.Id == payment.Id);

            if (paymentToUpdate.Status != "Success")
            {
                if (responseCode == "00" && transactionStatus == "00")
                {
                    paymentToUpdate.Status = "Paid";

                    paymentToUpdate.PaidAt = DateTime.UtcNow;
                    paymentToUpdate.UpdatedAt = DateTime.UtcNow;

                    _db.PaymentTransactions.Add(new PaymentTransaction
                    {
                        Id = Guid.NewGuid(),
                        PaymentRequestId = payment.Id,
                        TransactionId = string.IsNullOrEmpty(transactionNo) ? Guid.NewGuid().ToString() : transactionNo,
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

                    // Call UserService to mark request paid
                    var markPaidDto = new MarkPaidRequestDto
                    {
                        RequestId = payment.MembershipRequestId,
                        PaymentMethod = "VNPAY",
                        PaymentTransactionId = transactionNo ?? Guid.NewGuid().ToString(),
                        PaymentNote = "Auto updated via VNPay ReturnUrl",
                        PaymentProofUrl = null
                    };

                    try
                    {
                        await _resultHandler.HandleSuccessfulPaymentAsync(markPaidDto);
                        Console.WriteLine("✅ Đã notify thành công sang UserService");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("⚠️ Gọi sang UserService thất bại: " + ex.Message);
                    }
                }
                else
                {
                    paymentToUpdate.Status = "Failed";
                    paymentToUpdate.UpdatedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }

            // 5. Return JSON or redirect FE
            var returnUrl = payment.ReturnUrl;

            if (string.IsNullOrWhiteSpace(returnUrl) || returnUrl.Contains("/api/payments/vnpayreturn"))
            {
                return Ok(new
                {
                    message = "✅ Giao dịch đã được xử lý",
                    refCode = txnRef,
                    transactionNo,
                    status = paymentToUpdate.Status, // sẽ là "Paid"
                    code = responseCode
                });
            }

            return Redirect($"{returnUrl}?code={responseCode}&ref={txnRef}&status={paymentToUpdate.Status}&transactionNo={transactionNo}");
        }
    }
}
