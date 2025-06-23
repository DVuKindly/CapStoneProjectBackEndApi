using Microsoft.EntityFrameworkCore;
using PaymentService.API.Data;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;

using PaymentService.API.Entities;
using System;
using System.Threading.Tasks;

namespace PaymentService.API.Services
{
    public class PaymentWebhookService : IPaymentWebhookService
    {
        private readonly PaymentDbContext _db;
        private readonly IPaymentResultHandler _resultHandler;

        public PaymentWebhookService(PaymentDbContext db, IPaymentResultHandler resultHandler)
        {
            _db = db;
            _resultHandler = resultHandler;
        }

        public async Task<WebhookResult> HandleMomoWebhookAsync(MomoWebhookDto dto)
        {
            if (string.IsNullOrEmpty(dto.RequestCode) || string.IsNullOrEmpty(dto.TransactionId))
            {
                return new WebhookResult
                {
                    Success = false,
                    Message = "Thiếu RequestCode hoặc TransactionId."
                };
            }

            var request = await _db.PaymentRequests.FirstOrDefaultAsync(p => p.RequestCode == dto.RequestCode);
            if (request == null)
            {
                return new WebhookResult
                {
                    Success = false,
                    Message = $"Không tìm thấy Request với mã: {dto.RequestCode}"
                };
            }

            if (request.IsWebhookHandled || request.Status == "Paid")
            {
                return new WebhookResult
                {
                    Success = true,
                    Message = $"Request {dto.RequestCode} đã xử lý trước đó, không cần xử lý lại."
                };
            }

            var existingTransaction = await _db.PaymentTransactions
                .FirstOrDefaultAsync(t => t.TransactionId == dto.TransactionId);
            if (existingTransaction != null)
            {
                return new WebhookResult
                {
                    Success = true,
                    Message = $"TransactionId {dto.TransactionId} đã tồn tại, bỏ qua xử lý."
                };
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

            return new WebhookResult
            {
                Success = true,
                Message = $"Xử lý thành công cho RequestCode: {dto.RequestCode}"
            };
        }
    }
}
