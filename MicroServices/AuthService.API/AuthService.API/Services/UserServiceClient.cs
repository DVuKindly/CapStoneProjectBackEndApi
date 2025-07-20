using AuthService.API.DTOs.AdminCreate;
using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using AuthService.API.Services;
using System.Text;
using System.Text.Json;

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
                profilePayload.CityId = extra.CityId;
            }

            var normalizedRole = roleType.ToLowerInvariant();

            var endpoint = normalizedRole switch
            {
                "partner" => "/bff/api/user/profiles/create-partner",
                "coaching" or "coach" => "/bff/api/user/profiles/create-coach",
                "supplier" or "suppliers" => "/bff/api/user/profiles/create-supplier",
                "staff_service" or "staff_onboarding" or "staff_content" => "/bff/api/user/profiles/create-staff",
                _ => "/bff/api/user/profiles"
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
        var response = await _httpClient.GetAsync("/bff/api/user/locations");
        response.EnsureSuccessStatusCode();

        var locations = await response.Content.ReadFromJsonAsync<List<LocationDto>>();
        return locations ?? new();
    }
    public async Task<string?> GetLocationDisplayNameAsync(Guid propertyId)
    {
        var response = await _httpClient.GetAsync($"bff/api/user/locations/{propertyId}/display-name");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetLocationNameAsync(Guid locationId)
    {
        var locations = await GetLocationsAsync();
        var match = locations.FirstOrDefault(x => x.Id == locationId);
        return match?.Name;
    }

    public async Task<bool> IsValidLocationAsync(Guid locationId)
    {
        var response = await _httpClient.GetAsync($"/bff/api/user/locations/{locationId}/exists");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUserProfileStatusAsync(UserProfilePayload payload)
    {
        try
        {
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("/bff/api/user/profiles/update-status", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError("❌ Failed to update profile status → {StatusCode}: {Error}", response.StatusCode, error);
                return false;
            }

            _logger.LogInformation("✅ Profile status updated successfully for AccountId: {AccountId}", payload.AccountId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Exception occurred while calling UserService to update profile status");
            return false;
        }
    }

   

    public async Task<Dictionary<Guid, UserProfileDto>> GetProfilesByUserIdsAsync(IEnumerable<Guid> userIds)
    {
        var query = string.Join(",", userIds);
        var response = await _httpClient.GetAsync($"/bff/api/user/profiles/full-by-ids?ids={query}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<UserProfileDto>>();
        return result?.ToDictionary(p => p.AccountId) ?? new();
    }
    public async Task<List<UserProfileShortDto>> GetUserProfileShortDtosByIdsAsync(List<Guid> ids)
    {
        var query = string.Join(",", ids);
        var response = await _httpClient.GetAsync($"/bff/api/user/profiles/by-ids?ids={query}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<UserProfileShortDto>>();
        return result ?? new();
    }
    public async Task<List<UserProfileShortDto>> GetUserProfilesByRoleKeysAsync(string[] roleKeys)
    {
        var query = string.Join(",", roleKeys);
        var response = await _httpClient.GetAsync($"/bff/api/user/profiles/by-role-keys?roleKeys={query}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<UserProfileShortDto>>();
        return result ?? new();
    }

    public async Task<UserProfileShortDto?> GetUserProfileShortDtoByIdAsync(Guid accountId)
    {
        var response = await _httpClient.GetAsync($"/bff/api/user/profiles/by-id/{accountId}");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<UserProfileShortDto>();
        return result;
    }
    public async Task<bool> IsValidCityAsync(Guid cityId)
    {
        var response = await _httpClient.GetAsync($"/bff/api/user/cities/{cityId}/exists");
        return response.IsSuccessStatusCode;
    }


    public async Task<bool> IsPropertyInCityAsync(Guid propertyId, Guid cityId)
    {
        var response = await _httpClient.GetAsync($"/bff/api/user/locations/{propertyId}/in-city/{cityId}");
        return response.IsSuccessStatusCode;
    }


}
