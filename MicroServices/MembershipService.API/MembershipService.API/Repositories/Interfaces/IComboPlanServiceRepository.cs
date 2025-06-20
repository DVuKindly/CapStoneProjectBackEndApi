using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IComboPlanServiceRepository
    {
        Task AddRangeAsync(IEnumerable<ComboPlanService> services);
        Task<List<ComboPlanService>> GetByPackageIdAsync(Guid comboPlanId);
        Task DeleteByPackageIdAsync(Guid comboPlanId);
    }
}
