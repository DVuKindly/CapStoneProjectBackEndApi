using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AuthService.API.Services;
using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.AdminCreate;

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
        string roleType = "user",
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
                profilePayload.LocationId = extra.LocationId;
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

            var normalizedRole = roleType.ToLowerInvariant();

            var endpoint = normalizedRole switch
            {
                "partner" => "/api/userprofiles/create-partner",
                "coaching" or "coach" => "/api/userprofiles/create-coach",
                "supplier" or "suppliers" => "/api/userprofiles/create-supplier",
                "staff_service" or "staff_onboarding" or "staff_content" => "/api/userprofiles/create-staff",
                _ => "/api/userprofiles"
            };




            var jsonPayload = JsonSerializer.Serialize(profilePayload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            _logger.LogInformation("📤 Sending CreateUserProfile ({Role}) → {Endpoint}", roleType, endpoint);
            _logger.LogDebug("Payload: {Json}", jsonPayload);

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("❌ Failed to create profile → {StatusCode}: {Error}", response.StatusCode, error);
            }
            else
            {
                _logger.LogInformation("✅ Profile created successfully for {Email}", email);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Exception occurred while calling UserService to create profile");
        }






    }

    public async Task<List<LocationDto>> GetLocationsAsync()
    {
        var response = await _httpClient.GetAsync("/api/locations");
        response.EnsureSuccessStatusCode();

        var locations = await response.Content.ReadFromJsonAsync<List<LocationDto>>();
        return locations ?? new();
    }
    public async Task<bool> IsValidLocationAsync(Guid locationId)
    {
        var response = await _httpClient.GetAsync($"/api/locations/{locationId}/exists");
        return response.IsSuccessStatusCode;
    }

}
