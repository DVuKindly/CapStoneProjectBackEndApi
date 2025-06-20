using Microsoft.EntityFrameworkCore;
using PaymentService.API.Data;
using PaymentService.API.DTOs.Requests;
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

        public async Task<bool> HandleMomoWebhookAsync(MomoWebhookDto dto)
        {
            // Tìm theo RequestCode (string)
            var request = await _db.PaymentRequests
                .FirstOrDefaultAsync(p => p.RequestCode == dto.RequestCode);

            if (request == null || request.Status != "Pending")
                return false;

            request.Status = dto.Status == "Success" ? "Paid" : "Failed";
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

            if (dto.Status == "Success")
            {
                var markPaidDto = new MarkPaidRequestDto
                {
                    RequestId = request.MembershipRequestId,
                    PaymentMethod = "MOMO",
                    PaymentTransactionId = dto.TransactionId,
                    PaymentNote = dto.PaymentNote,
                    PaymentProofUrl = dto.PaymentProofUrl
                };

                await _resultHandler.HandleSuccessfulPaymentAsync(markPaidDto);
            }

            return true;
        }

    }
}
