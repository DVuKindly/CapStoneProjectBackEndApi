using AuthService.API.DTOs.Request;
using AuthService.API.Services;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

public class UserServiceClient : IUserServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserServiceClient> _logger;

    public UserServiceClient(HttpClient httpClient, ILogger<UserServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task CreateUserProfileAsync(
        Guid userId,
        string userName,
        string email,
        string roleType = "User",
        ProfileInfoRequest? profileInfo = null)
    {
        // Convert ProfileInfoRequest thành Dictionary<string, string>
        Dictionary<string, string> profileDict = new();
        if (profileInfo != null)
        {
            var json = JsonSerializer.Serialize(profileInfo, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            profileDict = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                          ?? new Dictionary<string, string>();
        }

        // Gộp toàn bộ thông tin thành payload động
        var payload = new Dictionary<string, object>
        {
            ["AccountId"] = userId,
            ["FullName"] = userName,
            ["Email"] = email,
            ["RoleType"] = roleType
        };

        // Merge thêm profileDict vào payload
        foreach (var kv in profileDict)
        {
            payload[kv.Key] = kv.Value;
        }

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        try
        {
            _logger.LogInformation("📤 Sending CreateUserProfile request: {Json}", jsonPayload);

            var response = await _httpClient.PostAsync("/api/userprofiles", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("❌ Failed to create user profile: {StatusCode} - {Error}", response.StatusCode, error);
            }
            else
            {
                _logger.LogInformation("✅ Successfully created user profile for {Email}", email);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Exception when calling UserService to create profile");
        }
    }
}
