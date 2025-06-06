using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AuthService.API.Services;
using AuthService.API.DTOs.Request;

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
        object? profileInfo = null)
    {
        try
        {
            var profilePayload = new UserProfilePayload
            {
                AccountId = userId,
                FullName = userName,
                Email = email,
                RoleType = roleType
            };

            if (profileInfo is UserProfilePayload extra)
            {
                profilePayload.Phone = extra.Phone;
                profilePayload.Gender = extra.Gender;
                profilePayload.DOB = extra.DOB;
                profilePayload.Location = extra.Location;
                profilePayload.OnboardingStatus = extra.OnboardingStatus;
                profilePayload.Note = extra.Note;

                profilePayload.OrganizationName = extra.OrganizationName;
                profilePayload.PartnerType = extra.PartnerType;
                profilePayload.ContractUrl = extra.ContractUrl;
                profilePayload.RepresentativeName = extra.RepresentativeName;
                profilePayload.RepresentativePhone = extra.RepresentativePhone;
                profilePayload.RepresentativeEmail = extra.RepresentativeEmail;
                profilePayload.Description = extra.Description;
                profilePayload.WebsiteUrl = extra.WebsiteUrl;
                profilePayload.Industry = extra.Industry;
                profilePayload.CreatedByAdminId = extra.CreatedByAdminId;

                profilePayload.CoachType = extra.CoachType;
                profilePayload.Module = extra.Module;
                profilePayload.Specialty = extra.Specialty;

                profilePayload.Level = extra.Level;
                profilePayload.Department = extra.Department;
                profilePayload.Address = extra.Address;
                profilePayload.ManagerId = extra.ManagerId;
            }

            var basicJson = JsonSerializer.Serialize(profilePayload);
            var basicContent = new StringContent(basicJson, Encoding.UTF8, "application/json");
            var preResponse = await _httpClient.PostAsync("/api/userprofiles", basicContent);

            if (!preResponse.IsSuccessStatusCode)
            {
                var err = await preResponse.Content.ReadAsStringAsync();
                _logger.LogWarning("⚠️ Could not pre-create user profile: {StatusCode} - {Error}", preResponse.StatusCode, err);
            }

            var endpoint = roleType switch
            {
                "partner" => "/api/userprofiles/create-partner",
                "coaching" => "/api/userprofiles/create-coach",
                "staff_service" or "staff_onboarding" => "/api/userprofiles/create-staff",
                _ => null
            };

            if (endpoint != null)
            {
                var jsonPayload = JsonSerializer.Serialize(profilePayload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                _logger.LogInformation("📤 Sending CreateUserProfile ({Role}) → {Endpoint}", roleType, endpoint);
                _logger.LogDebug("Payload: {Json}", jsonPayload);

                var response = await _httpClient.PostAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError("❌ Failed to create role profile → {StatusCode}: {Error}", response.StatusCode, error);
                }
                else
                {
                    _logger.LogInformation("✅ Role profile created successfully for {Email}", email);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Exception occurred while calling UserService to create profile");
        }
    }
}
