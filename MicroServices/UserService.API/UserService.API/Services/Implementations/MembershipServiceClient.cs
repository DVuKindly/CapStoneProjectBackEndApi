using System.Net.Http.Json;
using System.Text.Json;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Services.Interfaces;

public class MembershipServiceClient : IMembershipServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MembershipServiceClient> _logger;
    private readonly IConfiguration _config;

    public MembershipServiceClient(HttpClient httpClient, ILogger<MembershipServiceClient> logger, IConfiguration config)
    {
        _httpClient = httpClient;
        _logger = logger;
        _config = config;
    }

    public async Task<BasicPlanDto?> GetBasicPlanByIdAsync(Guid planId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/basicplans/{planId}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("[MembershipService] GetBasicPlanByIdAsync failed with status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<BasicPlanDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MembershipService] Exception in GetBasicPlanByIdAsync");
            return null;
        }
    }

    public async Task<List<BasicPlanResponse>> GetBasicPlansByIdsAsync(List<Guid> ids)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/basicplans/batch", ids);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<BasicPlanResponse>>() ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MembershipService] Exception in GetBasicPlansByIdsAsync");
            return new();
        }
    }

    public async Task<ComboPlanDto?> GetComboPlanByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/comboplans/{id}");
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("[MembershipService] GetComboPlanByIdAsync failed with status: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ComboPlanDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MembershipService] Exception in GetComboPlanByIdAsync");
            return null;
        }
    }

    public async Task<decimal> GetPlanPriceAsync(Guid planId, string planType)
    {
        try
        {
            string endpoint = planType.ToLower() switch
            {
                "basic" => $"/api/basicplans/{planId}/price",
                "combo" => $"/api/comboplans/{planId}/price",
                _ => throw new ArgumentException("Invalid plan type")
            };

            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("[MembershipService] GetPlanPriceAsync failed: {StatusCode}", response.StatusCode);
                return 0;
            }

            var content = await response.Content.ReadAsStringAsync();
            if (content.StartsWith("\"") && content.EndsWith("\""))
                content = content.Trim('"');

            if (decimal.TryParse(content, out var price))
                return price;

            _logger.LogError("[MembershipService] Failed to parse price: {Content}", content);
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MembershipService] Exception in GetPlanPriceAsync");
            return 0;
        }
    }

    public async Task<PlanPriceInfoDto?> GetPlanPriceInfoAsync(Guid planId, string planType)
    {
        try
        {
            string endpoint = planType.ToLower() switch
            {
                "basic" => $"/api/basicplans/{planId}/price",
                "combo" => $"/api/comboplans/{planId}/price",
                _ => throw new ArgumentException("Invalid plan type")
            };

            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("[MembershipService] GetPlanPriceInfoAsync failed: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<PlanPriceInfoDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MembershipService] Exception in GetPlanPriceInfoAsync");
            return null;
        }
    }
    public async Task<DurationDto?> GetPlanDurationAsync(Guid planId, string planType)
    {
        try
        {
            string endpoint = planType.ToLower() switch
            {
                "basic" => $"/api/basicplans/{planId}/duration",
                "combo" => $"/api/comboplans/{planId}/duration",
                _ => throw new ArgumentException("Invalid plan type")
            };

            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<DurationDto>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MembershipService] GetPlanDurationAsync Exception");
            return null;
        }
    }


}
