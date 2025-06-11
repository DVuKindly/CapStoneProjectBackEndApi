using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IPaymentServiceClient
    {
        Task<BaseResponse> CreatePaymentRequestAsync(CreatePaymentRequestDto dto);
    }

}
