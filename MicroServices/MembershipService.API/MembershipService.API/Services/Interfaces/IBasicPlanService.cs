using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IBasicPlanService
    {
        Task<BasicPlanResponseDto> CreateAsync(CreateBasicPlanRequest request);
        Task<BasicPlanResponseDto> UpdateAsync(Guid id, UpdateBasicPlanRequest request);
        Task<BasicPlanResponseDto> GetByIdAsync(Guid id);
        Task<List<BasicPlanResponseDto>> GetAllAsync();
        Task<bool> DeleteAsync(Guid id);
        Task<decimal> CalculateDynamicPriceFromRoomIdsAsync(List<Guid> roomIds);



        // vũ code
        Task<List<BasicPlanResponseDto>> GetByIdsAsync(List<Guid> ids);
        Task<DurationDto?> GetPlanDurationAsync(Guid planId);
        Task<bool> IsRoomBelongToPlanAsync(Guid planId, Guid roomInstanceId);


    }
}
