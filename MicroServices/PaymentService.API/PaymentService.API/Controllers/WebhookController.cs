using Microsoft.AspNetCore.Mvc;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;
using PaymentService.API.Helpers;
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

        [HttpGet("vnpay")]
        public async Task<IActionResult> VnPayWebhook()
        {
            Console.WriteLine("📥 Webhook VNPay được gọi");

            var dto = VnPayWebhookHelper.ToVnPayWebhookDto(HttpContext.Request.Query);
            var result = await _paymentWebhookService.HandleVnPayWebhookAsync(dto);

            // Bắt buộc trả về HTTP 200
            return Ok("IPN Handled");
        }


        [HttpPost("momo")]
        public async Task<IActionResult> MomoWebhook([FromBody] MomoWebhookDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Payload không được null" });

            var result = await _paymentWebhookService.HandleMomoWebhookAsync(dto);

            return result.Success
                ? Ok(new { message = result.Message })
                : BadRequest(new { message = result.Message });
        }
    }

}
