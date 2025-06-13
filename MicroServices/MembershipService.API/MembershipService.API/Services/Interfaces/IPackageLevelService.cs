using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IPackageLevelService
    {
        Task<List<PackageLevelResponse>> GetAllAsync();
        Task<PackageLevelResponse?> GetByIdAsync(Guid id);
        Task<PackageLevelResponse> CreateAsync(PackageLevelCreateRequest request);
        Task<bool> UpdateAsync(Guid id, PackageLevelUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
