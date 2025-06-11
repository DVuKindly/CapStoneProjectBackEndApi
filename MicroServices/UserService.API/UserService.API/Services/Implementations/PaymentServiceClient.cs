using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class PaymentServiceClient : IPaymentServiceClient
    {
        private readonly HttpClient _httpClient;

        public PaymentServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }





        public async Task<BaseResponse> CreatePaymentRequestAsync(CreatePaymentRequestDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/payment/create", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new BaseResponse { Success = false, Message = $"Gọi Payment API thất bại: {error}" };
            }

            var result = await response.Content.ReadFromJsonAsync<BaseResponse>();
            return result ?? new BaseResponse { Success = false, Message = "Lỗi không xác định." };
        }
    }
}
