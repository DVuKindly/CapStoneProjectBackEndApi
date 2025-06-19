using System.Net.Http.Json;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Services.Interfaces;

public class MembershipServiceClient : IMembershipServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MembershipServiceClient> _logger;

    public MembershipServiceClient(HttpClient httpClient, ILogger<MembershipServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
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



}

