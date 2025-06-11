using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;

namespace PaymentService.API.Services
{
    public interface IPaymentService
    {
        Task<BaseResponse> CreatePaymentRequestAsync(Guid accountId, CreatePaymentRequestDto dto);
    }

}
