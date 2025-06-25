using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.Data;
using SharedKernel.DTOsChung;

namespace PaymentService.API.Services
{
    public class PaymentResultHandler : IPaymentResultHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly PaymentDbContext _db;

        public PaymentResultHandler(HttpClient httpClient, IConfiguration config, PaymentDbContext db)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _db = db ?? throw new ArgumentNullException(nameof(db));
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

            // Luôn gọi endpoint /api/user/memberships/mark-paid
            var endpoint = $"{userServiceUrl.TrimEnd('/')}/api/user/memberships/mark-paid";

            Console.WriteLine($"📤 Gửi cập nhật thanh toán đến: {endpoint}");
            Console.WriteLine($"📝 RequestId = {dto.RequestId}");

            try
            {
                // Bỏ IsDirectMembership khỏi dto khi gửi đi nếu có
                var options = new JsonSerializerOptions
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                // Tạo một đối tượng mới bỏ IsDirectMembership
                var dtoToSend = new
                {
                    requestId = dto.RequestId,
                    membershipRequestId = dto.MembershipRequestId,
                    paymentTransactionId = dto.PaymentTransactionId,
                    paymentMethod = dto.PaymentMethod,
                    paymentNote = dto.PaymentNote,
                    paymentProofUrl = dto.PaymentProofUrl,
                    fullName = dto.FullName,
                    source = dto.Source
                };

                var json = JsonSerializer.Serialize(dtoToSend, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"❌ Gọi UserService thất bại - Mã: {response.StatusCode} - Nội dung: {errorContent}");
                }
                else
                {
                    Console.WriteLine($"✅ Đã cập nhật thành công cho RequestId: {dto.RequestId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Lỗi khi gọi UserService: {ex.Message}");
            }
        }
    }
}
