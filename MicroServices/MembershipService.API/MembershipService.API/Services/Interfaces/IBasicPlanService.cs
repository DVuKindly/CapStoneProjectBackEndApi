using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IBasicPlanService
    {
        Task<BasicPlanResponse> CreateAsync(BasicPlanCreateRequest request);
        Task<BasicPlanResponse?> GetByIdAsync(Guid id);
        Task<List<BasicPlanResponse>> GetAllAsync();
        Task<bool> UpdateAsync(Guid id, BasicPackageUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);


        // vũ code
        Task<List<BasicPlanResponse>> GetByIdsAsync(List<Guid> ids);
    }
}
