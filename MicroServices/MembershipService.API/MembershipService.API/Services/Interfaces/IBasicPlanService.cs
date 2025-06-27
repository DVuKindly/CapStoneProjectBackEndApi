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


        // vũ code
        Task<List<BasicPlanResponse>> GetByIdsAsync(List<Guid> ids);
        Task<DurationDto?> GetPlanDurationAsync(Guid planId);

    }
}
