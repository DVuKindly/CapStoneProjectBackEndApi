using Microsoft.AspNetCore.Mvc;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.Entities;
using PaymentService.API.Services;
using System.Security.Claims;
using SharedKernel.DTOsChung.Request;
using PaymentService.API.Data;
using PaymentService.API.DTOs.Response;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentWebhookService _paymentWebhookService;
        private readonly PaymentDbContext _db;

        public PaymentsController(IPaymentService paymentService, IPaymentWebhookService paymentWebhookService, PaymentDbContext db)
        {
            _paymentService = paymentService;
            _paymentWebhookService = paymentWebhookService;
            _db = db;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequestDto dto)
        {
            var accountId = GetAccountId();
            var result = await _paymentService.CreatePaymentRequestAsync(accountId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("webhook/momo")]
        public async Task<IActionResult> HandleMomoWebhook([FromBody] MomoReturnDto dto)
        {
            var request = await _db.PaymentRequests.FindAsync(dto.PaymentRequestId);
            if (request == null) return NotFound();

            var normalizedStatus = dto.PaymentStatus?.Trim().ToLowerInvariant();
            request.Status = normalizedStatus == "success" || normalizedStatus == "0"
                ? "Paid"
                : "Failed";

            if (request.Status == "Failed")
            {
                request.FailureReason = dto.Message ?? "Thanh toán thất bại";
                request.FailureCode = dto.ResultCode?.ToString() ?? "UNKNOWN";
            }

            request.IsWebhookHandled = true;
            request.WebhookHandledAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok();
        }

        private Guid GetAccountId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(idClaim, out var id) ? id : Guid.Empty;
        }
    }

  
}
