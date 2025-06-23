using Microsoft.AspNetCore.Mvc;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;

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
                return BadRequest(new { message = "Payload không được null" });

            WebhookResult result = await _paymentWebhookService.HandleMomoWebhookAsync(dto);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message ?? "Xử lý webhook không thành công"
                });
            }

            return Ok(new
            {
                message = result.Message ?? "Webhook xử lý thành công"
            });
        }
    }
}
