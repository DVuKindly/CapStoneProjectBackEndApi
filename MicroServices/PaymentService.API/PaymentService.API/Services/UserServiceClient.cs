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

    public async Task<bool> NotifyPaymentSuccessAsync(Guid membershipRequestId)
{
    var response = await _httpClient.PostAsJsonAsync("/bff/api/user/memberships/mark-paid", new MarkPaidRequestDto
    {
        MembershipRequestId = membershipRequestId
    });
    return response.IsSuccessStatusCode;
}


       
        public async Task<bool> MarkMembershipRequestAsPaidAsync(MarkPaidRequestDto dto)
        {
            var baseUrl = _config["UserService:BaseUrl"];
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/user/memberships/mark-paid", dto);
            return response.IsSuccessStatusCode;
        }
    }
}
