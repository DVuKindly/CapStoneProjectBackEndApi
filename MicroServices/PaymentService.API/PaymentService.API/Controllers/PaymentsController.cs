using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.Services;
using System.Security.Claims;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] CreatePaymentRequestDto dto)
        {
            // 👤 Lấy accountId từ token nếu cần
            var accountId = GetAccountId();
            var result = await _paymentService.CreatePaymentRequestAsync(accountId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        private Guid GetAccountId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(idClaim, out var id) ? id : Guid.Empty;
        }
    }

}
