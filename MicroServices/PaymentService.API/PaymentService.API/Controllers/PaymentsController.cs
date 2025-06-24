using Microsoft.AspNetCore.Mvc;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;
using PaymentService.API.Entities;
using PaymentService.API.Services;
using PaymentService.API.Data;
using System.Security.Claims;
using PaymentService.API.Helpers;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentWebhookService _paymentWebhookService;
        private readonly PaymentDbContext _db;

        public PaymentsController(
            IPaymentService paymentService,
            IPaymentWebhookService paymentWebhookService,
            PaymentDbContext db)
        {
            _paymentService = paymentService;
            _paymentWebhookService = paymentWebhookService;
            _db = db;
        }

        // 🎯 Tạo yêu cầu thanh toán
        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequestDto dto)
        {
            var accountId = GetAccountId();
            var result = await _paymentService.CreatePaymentRequestAsync(accountId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

       
      



        // ✅ Trích xuất accountId từ JWT
        private Guid GetAccountId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(idClaim, out var id) ? id : Guid.Empty;
        }
    }
}
