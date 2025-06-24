using System.Net.Http.Json;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Services.Interfaces;
using static System.Net.WebRequestMethods;

public class MembershipServiceClient : IMembershipServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MembershipServiceClient> _logger;
    private readonly HttpClient _http;


    public MembershipServiceClient(HttpClient httpClient, ILogger<MembershipServiceClient> logger, HttpClient http)
    {
        _httpClient = httpClient;
        _logger = logger;
        _http = http;
    }

    public async Task<BasicPlanDto?> GetBasicPlanByIdAsync(Guid planId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/basicplans/{planId}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<BasicPlanDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to call MembershipService to get basic plan");
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
            _logger.LogError(ex, "Failed to get basic plans by ids from MembershipService");
            return new List<BasicPlanResponse>();
        }
    }
    public async Task<ComboPlanDto?> GetComboPlanByIdAsync(Guid id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/comboplans/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ComboPlanDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to call MembershipService to get combo plan");
            return null;
        }
    }

    public async Task<decimal> GetPlanPriceAsync(Guid planId, string planType)
    {
        var endpoint = planType.ToLower() switch
        {
            "basic" => $"/api/basicplans/{planId}/price",
            "combo" => $"/api/comboplans/{planId}/price",
            _ => throw new ArgumentException("Invalid plan type")
        };

        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var price = decimal.Parse(json); // hoặc dùng JsonSerializer

        return price;
    }



}

