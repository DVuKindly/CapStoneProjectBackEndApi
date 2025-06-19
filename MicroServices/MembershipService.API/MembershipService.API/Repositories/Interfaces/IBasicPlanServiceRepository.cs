using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBasicPlanServiceRepository
    {
        Task AddRangeAsync(IEnumerable<BasicPlanService> services);
        Task<List<BasicPlanService>> GetByPackageIdAsync(Guid packageId);
        Task DeleteByPackageIdAsync(Guid packageId);
    }
}
