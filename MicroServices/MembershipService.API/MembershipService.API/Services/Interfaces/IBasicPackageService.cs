using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IBasicPackageService
    {
        Task<BasicPackageResponse> CreateAsync(BasicPackageCreateRequest request);
        Task<BasicPackageResponse?> GetByIdAsync(Guid id);
        Task<List<BasicPackageResponse>> GetAllAsync();
        Task<bool> UpdateAsync(Guid id, BasicPackageUpdateRequest request);
        Task<bool> DeleteAsync(Guid id);
    }
}
