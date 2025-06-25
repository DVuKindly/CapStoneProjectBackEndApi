using System.Net.Http.Json;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using SharedKernel.DTOsChung;

namespace PaymentService.API.Services
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public UserServiceClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<bool> NotifyPaymentSuccessAsync(Guid paymentRequestId, Guid? membershipRequestId = null, bool? isDirectMembership = null)
        {
            var dto = new MarkPaidRequestDto
            {
                RequestId = paymentRequestId,
                MembershipRequestId = membershipRequestId,
                IsDirectMembership = isDirectMembership
            };

            return await MarkMembershipRequestAsPaidAsync(dto);
        }






        public async Task<bool> MarkMembershipRequestAsPaidAsync(MarkPaidRequestDto dto)
        {
            var baseUrl = _config["UserService:BaseUrl"];

            // Auto detect nếu chưa có
            if (dto.IsDirectMembership == null)
            {
                Console.WriteLine("⚠️ IsDirectMembership chưa được truyền – cần xử lý tự động từ DB ở ngoài");
                return false; // hoặc throw exception tùy logic
            }

            var endpoint = dto.IsDirectMembership == true
                ? "/api/user/memberships/mark-paid-membership"
                : "/api/user/memberships/mark-paid";

            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}{endpoint}", dto);
            return response.IsSuccessStatusCode;
        }

    }
}
