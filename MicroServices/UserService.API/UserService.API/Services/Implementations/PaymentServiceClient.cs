using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Services.Interfaces;
using System.Net.Http.Json;

namespace UserService.API.Services.Implementations
{
    public class PaymentServiceClient : IPaymentServiceClient
    {
        private readonly HttpClient _httpClient;

        public PaymentServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("X-Internal-Call", "true");
        }

        public async Task<BaseResponse> CreatePaymentRequestAsync(CreatePaymentRequestDto dto)
        {
            try
            {
             
                var response = await _httpClient.PostAsJsonAsync("/api/payments/create", dto);

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("❗Response từ PaymentService: " + content);

                if (!response.IsSuccessStatusCode)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Gọi Payment API thất bại: " + content
                    };
                }

                var result = await response.Content.ReadFromJsonAsync<BaseResponse>();
                return result ?? new BaseResponse
                {
                    Success = false,
                    Message = "Không thể đọc phản hồi từ PaymentService."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Gọi Payment API lỗi exception: " + ex.Message
                };
            }
        }
    }
}
