using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IComboPlanService
    {
        Task<ComboPlanResponseDto> CreateAsync(CreateComboPlanRequest request);
        Task<ComboPlanResponseDto> GetByIdAsync(Guid id);
        Task<List<ComboPlanResponseDto>> GetAllAsync();
        Task<ComboPlanResponseDto> UpdateAsync(Guid id, UpdateComboPlanRequest request);
        Task<bool> DeleteAsync(Guid id);
        //vũ code 
        Task<DurationDto?> GetPlanDurationAsync(Guid planId);
        Task<List<ComboPlanResponseDto>> GetByIdsAsync(List<Guid> ids);

    }
}
