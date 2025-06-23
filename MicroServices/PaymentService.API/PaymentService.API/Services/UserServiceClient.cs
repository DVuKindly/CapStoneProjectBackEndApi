using System.Net.Http.Json;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.Services.Interfaces;

namespace PaymentService.API.Services
{
  

    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;

        public UserServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> MarkMembershipRequestAsPaidAsync(MarkPaidRequestDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/bff/api/user/memberships/mark-paid", dto);
            return response.IsSuccessStatusCode;
        }
    }

}
