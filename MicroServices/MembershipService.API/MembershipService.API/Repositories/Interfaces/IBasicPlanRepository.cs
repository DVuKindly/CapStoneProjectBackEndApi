using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBasicPlanRepository
    {
        Task<BasicPlan> CreateAsync(BasicPlan package);
        Task<BasicPlan?> GetByIdAsync(Guid id);
        Task<List<BasicPlan>> GetAllAsync();
        Task UpdateAsync(BasicPlan package);
        Task DeleteAsync(BasicPlan package);
    }
}
