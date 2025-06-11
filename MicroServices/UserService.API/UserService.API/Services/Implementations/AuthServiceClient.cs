using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class AuthServiceClient : IAuthServiceClient
    {
        private readonly HttpClient _http;

        public AuthServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> PromoteUserToMemberAsync(Guid accountId)
        {
            var response = await _http.PostAsJsonAsync("/api/auth/promote-to-member", new { AccountId = accountId });
            return response.IsSuccessStatusCode;
        }
    }

}
