using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBasicPackageServiceRepository
    {
        Task AddRangeAsync(IEnumerable<BasicPackageService> services);
        Task<List<BasicPackageService>> GetByPackageIdAsync(Guid packageId);
        Task DeleteByPackageIdAsync(Guid packageId);
    }
}
