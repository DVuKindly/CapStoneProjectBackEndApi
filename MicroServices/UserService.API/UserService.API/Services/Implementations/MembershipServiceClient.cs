﻿using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.DTOs.SyncPosition;
using UserService.API.Services.Interfaces;
using Microsoft.Extensions.Logging;

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
    public async Task<decimal> GetAddOnFee(Guid roomInstanceId)
    {
        var response = await _httpClient.GetAsync($"/api/roominstances/{roomInstanceId}/extra-fee");

        if (!response.IsSuccessStatusCode)
            throw new Exception("Failed to retrieve add-on fee from MembershipService.");

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<BaseResponse<decimal>>(content);

        if (result == null || !result.Success)
            throw new Exception("Invalid response when getting add-on fee.");

        return result.Data;
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
    public async Task<bool> CreateBookingAsync(
    Guid accountId,
    Guid roomInstanceId,
    DateTime startDate,
    int durationValue,
    string durationUnit)
    {
        var endDate = durationUnit.ToLower() switch
        {
            "day" => startDate.AddDays(durationValue),
            "month" => startDate.AddMonths(durationValue),
            "year" => startDate.AddYears(durationValue),
            _ => startDate
        };

        var request = new CreateBookingRequest
        {
            MemberId = accountId,
            RoomInstanceId = roomInstanceId,
            StartDate = startDate,
            EndDate = endDate,
            Note = "Tự động tạo sau khi thanh toán"
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/bookings", request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MembershipService] CreateBookingAsync failed.");
            return false;
        }
    }




    public async Task<bool> IsRoomBelongToPlanAsync(Guid planId, Guid roomInstanceId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/basicplans/{planId}/rooms/{roomInstanceId}/check");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("[MembershipService] IsRoomBelongToPlanAsync failed with status: {StatusCode}", response.StatusCode);
                return false;
            }

            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MembershipService] Exception in IsRoomBelongToPlanAsync");
            return false;
        }
    }

    public async Task<bool> IsRoomBookedAsync(Guid roomInstanceId, DateTime startDate, DateTime endDate)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/bookings/check?roomId={roomInstanceId}&startDate={startDate:O}&endDate={endDate:O}");
            return response.IsSuccessStatusCode && await response.Content.ReadFromJsonAsync<bool>();
        }
        catch
        {
            return false;
        }
    }
    public async Task<List<ComboPlanResponse>> GetComboPlansByIdsAsync(List<Guid> ids)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/comboplans/batch", ids);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ComboPlanResponse>>() ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[MembershipService] Exception in GetComboPlansByIdsAsync");
            return new();
        }
    }

    public async Task SyncCityAsync(SyncCityDto dto)
    {
        await _httpClient.PostAsJsonAsync("/api/membership/syncPosition/cities", dto);
    }

    public async Task SyncLocationAsync(SyncLocationDto dto)
    {
        await _httpClient.PostAsJsonAsync("/api/membership/syncPosition/locations", dto);
    }

    public async Task SyncPropertyAsync(SyncPropertyDto dto)
    {
        await _httpClient.PostAsJsonAsync("/api/membership/syncPosition/properties", dto);
    }

    public async Task DeleteCityAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"/api/membership/syncPosition/cities/{id}");
    }

    public async Task DeleteLocationAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"/api/membership/syncPosition/locations/{id}");
    }

    public async Task DeletePropertyAsync(Guid id)
    {
        await _httpClient.DeleteAsync($"/api/membership/syncPosition/properties/{id}");
    }
    public async Task<bool> CreateHoldBookingAsync(HoldBookingRequestDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/bookings/hold", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateHoldBookingAsync failed.");
            return false;
        }
    }

    public async Task<bool> ConfirmBookingAsync(ConfirmBookingRequestDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/bookings/confirm", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ConfirmBookingAsync failed.");
            return false;
        }



     
    }
    public async Task<bool> CancelHoldBookingAsync(Guid memberId, Guid roomInstanceId, DateTime startDate)
    {
        var dto = new CancelHoldBookingRequest
        {
            MemberId = memberId,
            RoomInstanceId = roomInstanceId,
            StartDate = startDate
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/bookings/cancel-hold", dto);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CancelHoldBookingAsync failed.");
            return false;
        }
    }

}
