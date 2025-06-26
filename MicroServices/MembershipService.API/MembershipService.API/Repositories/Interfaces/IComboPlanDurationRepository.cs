using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IComboPlanDurationRepository
    {
        Task<List<ComboPlanDuration>> GetByBasicPlanIdAsync(Guid basicPlanId);
        Task<List<ComboPlanDuration>> GetByComboPlanIdAsync(Guid comboPlanId); 
        Task AddRangeAsync(List<ComboPlanDuration> durations);
        Task RemoveByBasicPlanIdAsync(Guid basicPlanId);
        Task RemoveByComboPlanIdAsync(Guid comboPlanId); 
    }
}
