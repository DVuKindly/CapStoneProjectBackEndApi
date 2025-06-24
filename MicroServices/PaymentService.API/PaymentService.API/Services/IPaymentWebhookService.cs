using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;

using System.Threading.Tasks;

namespace PaymentService.API.Services
{
    public interface IPaymentWebhookService
    {
        Task<WebhookResult> HandleMomoWebhookAsync(MomoWebhookDto dto);
        Task<WebhookResult> HandleVnPayWebhookAsync(VnPayWebhookDto dto);
    }

}
