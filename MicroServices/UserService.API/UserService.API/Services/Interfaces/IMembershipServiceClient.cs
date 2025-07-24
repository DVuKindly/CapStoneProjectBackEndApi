using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.DTOs.SyncPosition;

namespace UserService.API.Services.Interfaces
{
    public interface IMembershipServiceClient
    {
        Task<BasicPlanDto?> GetBasicPlanByIdAsync(Guid id);
        Task<List<BasicPlanResponse>> GetBasicPlansByIdsAsync(List<Guid> ids);

        Task<ComboPlanDto?> GetComboPlanByIdAsync(Guid id);
        Task<decimal> GetAddOnFee(Guid roomInstanceId);

        Task<decimal> GetPlanPriceAsync(Guid planId, string planType);
        Task<PlanPriceInfoDto?> GetPlanPriceInfoAsync(Guid planId, string planType);

        Task<DurationDto?> GetPlanDurationAsync(Guid planId, string planType);

        Task<bool> IsRoomBelongToPlanAsync(Guid planId, Guid roomInstanceId);

        Task<bool> IsRoomBookedAsync(Guid roomInstanceId, DateTime startDate, DateTime endDate);

        Task<List<ComboPlanResponse>> GetComboPlansByIdsAsync(List<Guid> ids);

        Task<bool> CreateBookingAsync(
      Guid accountId,
      Guid roomInstanceId,
      DateTime startDate,
      int durationValue,
      string durationUnit);
        Task SyncCityAsync(SyncCityDto dto);
        Task SyncLocationAsync(SyncLocationDto dto);
        Task SyncPropertyAsync(SyncPropertyDto dto);
        Task DeleteCityAsync(Guid id);
        Task DeleteLocationAsync(Guid id);
        Task DeletePropertyAsync(Guid id);
        Task<bool> CreateHoldBookingAsync(HoldBookingRequestDto dto);
        Task<bool> ConfirmBookingAsync(ConfirmBookingRequestDto dto);

        Task<bool> CancelHoldBookingAsync(Guid memberId, Guid roomInstanceId, DateTime startDate);

    }
}
