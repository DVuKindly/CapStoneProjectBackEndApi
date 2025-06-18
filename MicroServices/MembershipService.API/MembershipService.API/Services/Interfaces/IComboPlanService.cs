using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IComboPlanService
    {
        Task<ComboPlanResponse> CreateAsync(ComboPlanCreateRequest request);
        Task<ComboPlanResponse?> GetByIdAsync(Guid id);
        Task<List<ComboPlanResponse>> GetAllAsync();
        Task<bool> UpdateAsync(Guid id, ComboPlanUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
