using Microsoft.AspNetCore.Mvc;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.Services;
using System.Threading.Tasks;

namespace PaymentService.API.Controllers
{
    [Route("api/webhook")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly IPaymentWebhookService _paymentWebhookService;

        public WebhookController(IPaymentWebhookService paymentWebhookService)
        {
            _paymentWebhookService = paymentWebhookService;
        }

        [HttpPost("momo")]
        public async Task<IActionResult> MomoWebhook([FromBody] MomoWebhookDto dto)
        {
            if (dto == null)
                return BadRequest("Payload null");

            var result = await _paymentWebhookService.HandleMomoWebhookAsync(dto);

            if (!result)
                return BadRequest("Xử lý webhook không thành công");

            return Ok("Webhook xử lý thành công");
        }
    }
}
