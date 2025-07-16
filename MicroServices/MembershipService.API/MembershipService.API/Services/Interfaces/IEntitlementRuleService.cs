using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IEntitlementRuleService
    {
        Task<List<EntitlementRuleDto>> GetAllAsync();
        Task<EntitlementRuleDto> GetByIdAsync(Guid id);
        Task<EntitlementRuleDto> CreateAsync(CreateEntitlementRuleDto dto);
        Task<EntitlementRuleDto> UpdateAsync(UpdateEntitlementRuleDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<List<EntitlementRuleDto>> GetByBasicPlanIdAsync(Guid basicPlanId);
        Task<decimal> GetTotalEntitlementPriceAsync(Guid basicPlanId);

    }
}
