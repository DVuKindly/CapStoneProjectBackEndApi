// IPaymentResultHandler.cs
using PaymentService.API.DTOs.Requests;

namespace PaymentService.API.Services
{
    public interface IPaymentResultHandler
    {
        Task HandleSuccessfulPaymentAsync(MarkPaidRequestDto dto);
    }
}
