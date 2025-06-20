// PaymentResultHandler.cs
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PaymentService.API.DTOs.Requests;

namespace PaymentService.API.Services
{
    public class PaymentResultHandler : IPaymentResultHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public PaymentResultHandler(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task HandleSuccessfulPaymentAsync(MarkPaidRequestDto dto)
        {
            if (dto == null || dto.RequestId == Guid.Empty)
            {
                Console.WriteLine("⚠️ MarkPaidRequestDto hoặc RequestId không hợp lệ");
                return;
            }

            var userServiceUrl = _config["UserService:BaseUrl"];
            if (string.IsNullOrEmpty(userServiceUrl))
            {
                Console.WriteLine("⚠️ Không tìm thấy cấu hình UserService:BaseUrl trong appsettings.");
                return;
            }

            var endpoint = $"{userServiceUrl.TrimEnd('/')}/api/membership/mark-paid";

            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(dto);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Gọi UserService thất bại với mã: {response.StatusCode} - Nội dung: {errorContent}");
                }
                else
                {
                    Console.WriteLine($"✅ Đã cập nhật MembershipRequest {dto.RequestId} thanh toán thành công.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Lỗi khi gọi UserService: {ex.Message}");
            }
        }
    }
}
