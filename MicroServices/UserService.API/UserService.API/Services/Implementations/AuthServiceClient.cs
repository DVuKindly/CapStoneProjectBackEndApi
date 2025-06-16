using System.Net.Http.Headers;
using System.Net.Http.Json;
using UserService.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace UserService.API.Services.Implementations
{


    public class AuthServiceClient : IAuthServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AuthServiceClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["InternalApi:ApiKey"] ?? throw new ArgumentNullException("InternalApi:ApiKey");
        }

        public async Task<bool> PromoteUserToMemberAsync(Guid accountId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/internal/promote-to-member");
            // Thêm header API Key cho request
            request.Headers.Add("X-Api-Key", _apiKey);

            request.Content = new StringContent(JsonSerializer.Serialize(new { AccountId = accountId }), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }

    public class PromoteToMemberRequest
    {
        public Guid AccountId { get; set; }
    }

}




