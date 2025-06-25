using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PaymentService.API.Services.Interfaces;
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

  
        public async Task<bool> NotifyPaymentSuccessAsync(Guid paymentRequestId, Guid? membershipRequestId = null)
        {
            var dto = new MarkPaidRequestDto
            {
                RequestId = paymentRequestId,
                MembershipRequestId = membershipRequestId,
               
            };

            return await MarkMembershipRequestAsPaidAsync(dto);
        }

        public async Task<bool> MarkMembershipRequestAsPaidAsync(MarkPaidRequestDto dto)
        {
            var baseUrl = _config["UserService:BaseUrl"];
            var endpoint = "/api/user/memberships/mark-paid"; 

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{baseUrl}{endpoint}", dto);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"⚠️ Gọi API {endpoint} thất bại: {response.StatusCode}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Exception khi gọi API UserService: " + ex.Message);
                return false;
            }
        }
    }
}
