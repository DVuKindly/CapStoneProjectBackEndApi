using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBasicPlanEntitlementRepository
    {
        Task<BasicPlanEntitlement?> AddAsync(BasicPlanEntitlement entity);
        Task<IEnumerable<BasicPlanEntitlement>> AddRangeAsync(IEnumerable<BasicPlanEntitlement> entities);
        Task<IEnumerable<BasicPlanEntitlement>> GetByBasicPlanIdAsync(Guid basicPlanId);
        Task DeleteByBasicPlanIdAsync(Guid basicPlanId);
    }
}
