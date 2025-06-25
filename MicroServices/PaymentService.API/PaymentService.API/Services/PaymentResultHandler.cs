using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PaymentService.API.DTOs.Requests;
using SharedKernel.DTOsChung;
using PaymentService.API.Data;
using Microsoft.EntityFrameworkCore;

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

            // ✅ Tự động xác định IsDirectMembership nếu null
            if (dto.IsDirectMembership == null)
            {

                var isDirect = await _db.PaymentRequests
    .Where(p => p.Id == dto.RequestId) // 👈 KHÔNG phải MembershipRequestId!
    .Select(p => p.IsDirectMembership)
    .FirstOrDefaultAsync();




                dto.IsDirectMembership = isDirect;

                Console.WriteLine($"🔍 Auto-detected IsDirectMembership = {isDirect}");
            }

            // ✅ Chọn endpoint phù hợp
            var endpoint = dto.IsDirectMembership == true
                ? $"{userServiceUrl.TrimEnd('/')}/api/user/memberships/mark-paid-membership"
                : $"{userServiceUrl.TrimEnd('/')}/api/user/memberships/mark-paid";

            Console.WriteLine($"📤 Gửi cập nhật thanh toán đến: {endpoint}");
            Console.WriteLine($"📝 RequestId = {dto.RequestId}, IsDirect = {dto.IsDirectMembership}");

            try
            {
                var json = JsonSerializer.Serialize(dto);
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
