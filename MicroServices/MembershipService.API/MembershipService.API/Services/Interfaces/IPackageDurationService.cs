using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IPackageDurationService
    {
        Task<IEnumerable<PackageDurationResponse>> GetAllAsync();
        Task<PackageDurationResponse?> GetByIdAsync(int id);
    }
}
