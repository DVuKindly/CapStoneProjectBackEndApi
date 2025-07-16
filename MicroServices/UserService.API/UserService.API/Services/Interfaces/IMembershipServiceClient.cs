using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;

namespace UserService.API.Services.Interfaces
{
    public interface IMembershipServiceClient
    {
        Task<BasicPlanDto?> GetBasicPlanByIdAsync(Guid id);
        Task<List<BasicPlanResponse>> GetBasicPlansByIdsAsync(List<Guid> ids);

        Task<ComboPlanDto?> GetComboPlanByIdAsync(Guid id);

        Task<decimal> GetPlanPriceAsync(Guid planId, string planType);
        Task<PlanPriceInfoDto?> GetPlanPriceInfoAsync(Guid planId, string planType);

        Task<DurationDto?> GetPlanDurationAsync(Guid planId, string planType);

        Task<bool> IsRoomBelongToPlanAsync(Guid planId, Guid roomInstanceId);

        Task<bool> IsRoomBookedAsync(Guid roomInstanceId, DateTime startDate, DateTime endDate);


        Task<bool> CreateBookingAsync(
      Guid accountId,
      Guid roomInstanceId,
      DateTime startDate,
      int durationValue,
      string durationUnit);


    }
}
