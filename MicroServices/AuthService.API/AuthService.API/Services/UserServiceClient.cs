using AuthService.API.Services;
using System.Text;
using System.Text.Json;
using System.Net.Http;

public class UserServiceClient : IUserServiceClient
{
    private readonly HttpClient _httpClient;

    public UserServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CreateUserProfileAsync(Guid accountId, string fullName, string email)
    {
        var payload = new
        {
            AccountId = accountId,
            FullName = fullName,
            Email = email
        };

        var json = JsonSerializer.Serialize(payload);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/api/userprofiles", content);

        return response.IsSuccessStatusCode;
    }
}
