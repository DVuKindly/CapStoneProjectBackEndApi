using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;

using System.Threading.Tasks;

namespace PaymentService.API.Services
{
    public interface IPaymentWebhookService
    {
        // Xử lý webhook từ Momo và trả về kết quả chi tiết
        Task<WebhookResult> HandleMomoWebhookAsync(MomoWebhookDto dto);
    }
}
