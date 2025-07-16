using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IEntitlementRuleRepository
    {
        Task<EntitlementRule> GetByIdAsync(Guid id);
        Task<List<EntitlementRule>> GetAllAsync();
        Task<EntitlementRule> AddAsync(EntitlementRule entity);
        Task<bool> UpdateAsync(EntitlementRule entity);
        Task<bool> DeleteAsync(Guid id);
        Task<List<EntitlementRule>> GetByBasicPlanIdAsync(Guid basicPlanId);
        Task<decimal> GetTotalEntitlementPriceAsync(Guid basicPlanId);


    }
}
