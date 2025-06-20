using PaymentService.API.DTOs.Requests;
using System.Threading.Tasks;

namespace PaymentService.API.Services
{
    public interface IPaymentWebhookService
    {
        // Xử lý webhook từ Momo
        Task<bool> HandleMomoWebhookAsync(MomoWebhookDto dto);

      
    }
}
