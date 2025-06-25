using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PaymentService.API.Data;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;
using PaymentService.API.Entities;
using PaymentService.API.Helpers;
using SharedKernel.DTOsChung;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.API.Services
{
    public class PaymentWebhookService : IPaymentWebhookService
    {
        private readonly PaymentDbContext _db;
        private readonly IPaymentResultHandler _resultHandler;
        private readonly IConfiguration _config;

        public PaymentWebhookService(PaymentDbContext db, IPaymentResultHandler resultHandler, IConfiguration config)
        {
            _db = db;
            _resultHandler = resultHandler;
            _config = config;
        }

        public async Task<WebhookResult> HandleVnPayWebhookAsync(VnPayWebhookDto dto)
        {
            if (string.IsNullOrEmpty(dto.vnp_TxnRef) || string.IsNullOrEmpty(dto.vnp_TransactionNo))
            {
                return new WebhookResult { Success = false, Message = "Thiếu mã đơn hàng hoặc mã giao dịch." };
            }

            var request = await _db.PaymentRequests.FirstOrDefaultAsync(p => p.RequestCode == dto.vnp_TxnRef);
            if (request == null)
            {
                return new WebhookResult { Success = false, Message = $"Không tìm thấy Request với mã: {dto.vnp_TxnRef}" };
            }

            if (request.IsWebhookHandled || request.Status == "Paid")
            {
                return new WebhookResult { Success = true, Message = $"Request {dto.vnp_TxnRef} đã xử lý trước đó." };
            }

            var existingTransaction = await _db.PaymentTransactions
                .FirstOrDefaultAsync(t => t.TransactionId == dto.vnp_TransactionNo);
            if (existingTransaction != null)
            {
                return new WebhookResult { Success = true, Message = $"TransactionId {dto.vnp_TransactionNo} đã tồn tại." };
            }

            var secret = _config["VNPay:HashSecret"];
            var vnp = new VnPayLibrary();
            foreach (var kv in dto.RawData)
            {
                if (kv.Key.StartsWith("vnp_"))
                    vnp.AddResponseData(kv.Key, kv.Value);
            }

            var checkHash = vnp.ValidateSignature(dto.vnp_SecureHash, secret);
            if (!checkHash)
            {
                return new WebhookResult { Success = false, Message = "Sai chữ ký hash (vnp_SecureHash)." };
            }

            var isSuccess = dto.vnp_ResponseCode == "00";
            request.Status = isSuccess ? "Paid" : "Failed";
            request.PaidAt = DateTime.UtcNow;
          

            request.IsWebhookHandled = true;
            request.WebhookHandledAt = DateTime.UtcNow;

            var transaction = new PaymentTransaction
            {
                Id = Guid.NewGuid(),
                PaymentRequestId = request.Id,
                TransactionId = dto.vnp_TransactionNo,
                Gateway = "vnpay",
                Status = dto.vnp_ResponseCode,
                GatewayResponse = string.Join("&", dto.RawData.Select(kv => $"{kv.Key}={kv.Value}")),
                CreatedAt = DateTime.UtcNow
            };

            _db.PaymentTransactions.Add(transaction);
            await _db.SaveChangesAsync();

            if (isSuccess)
            {
                var markPaidDto = new MarkPaidRequestDto
                {
                    RequestId = request.Id, // PaymentRequest.Id mới đúng
                    MembershipRequestId = request.MembershipRequestId,
                 
                    PaymentMethod = "VNPAY",
                    PaymentTransactionId = dto.vnp_TransactionNo,
                    PaymentNote = "Auto updated via VNPAY",
                    PaymentProofUrl = null
                };



                await _resultHandler.HandleSuccessfulPaymentAsync(markPaidDto);


                try
                {
                    await _resultHandler.HandleSuccessfulPaymentAsync(markPaidDto);
                }
                catch (Exception ex)
                {
                    return new WebhookResult
                    {
                        Success = false,
                        Message = $"Đã lưu giao dịch nhưng lỗi khi cập nhật trạng thái thành viên: {ex.Message}"
                    };
                }
            }

            return new WebhookResult
            {
                Success = true,
                Message = $"Xử lý thành công webhook cho mã đơn: {dto.vnp_TxnRef}"
            };
        }

        public async Task<WebhookResult> HandleMomoWebhookAsync(MomoWebhookDto dto)
        {
            if (string.IsNullOrEmpty(dto.RequestCode) || string.IsNullOrEmpty(dto.TransactionId))
            {
                return new WebhookResult { Success = false, Message = "Thiếu RequestCode hoặc TransactionId." };
            }

            var request = await _db.PaymentRequests.FirstOrDefaultAsync(p => p.RequestCode == dto.RequestCode);
            if (request == null)
            {
                return new WebhookResult { Success = false, Message = $"Không tìm thấy Request với mã: {dto.RequestCode}" };
            }

            if (request.IsWebhookHandled || request.Status == "Paid")
            {
                return new WebhookResult { Success = true, Message = $"Request {dto.RequestCode} đã xử lý trước đó." };
            }

            var existingTransaction = await _db.PaymentTransactions
                .FirstOrDefaultAsync(t => t.TransactionId == dto.TransactionId);
            if (existingTransaction != null)
            {
                return new WebhookResult { Success = true, Message = $"TransactionId {dto.TransactionId} đã tồn tại." };
            }

            var isSuccess = dto.Status?.ToLower() == "success";
            request.Status = isSuccess ? "Paid" : "Failed";
            request.PaidAt = DateTime.UtcNow;
            request.IsWebhookHandled = true;
            request.WebhookHandledAt = DateTime.UtcNow;

            var transaction = new PaymentTransaction
            {
                Id = Guid.NewGuid(),
                PaymentRequestId = request.Id,
                TransactionId = dto.TransactionId,
                Gateway = "momo",
                Status = dto.Status,
                GatewayResponse = dto.RawResponse,
                CreatedAt = DateTime.UtcNow
            };

            _db.PaymentTransactions.Add(transaction);
            await _db.SaveChangesAsync();

            if (isSuccess)
            {
                var markPaidDto = new MarkPaidRequestDto
                {
                    RequestId = request.MembershipRequestId,
                    PaymentMethod = "MOMO",
                    PaymentTransactionId = dto.TransactionId,
                    PaymentNote = dto.PaymentNote,
                    PaymentProofUrl = dto.PaymentProofUrl
                };

                try
                {
                    await _resultHandler.HandleSuccessfulPaymentAsync(markPaidDto);
                }
                catch (Exception ex)
                {
                    return new WebhookResult
                    {
                        Success = false,
                        Message = $"Đã cập nhật PaymentRequest nhưng lỗi khi gọi UserService: {ex.Message}"
                    };
                }
            }

            return new WebhookResult { Success = true, Message = $"Xử lý thành công cho RequestCode: {dto.RequestCode}" };
        }
    }
}
