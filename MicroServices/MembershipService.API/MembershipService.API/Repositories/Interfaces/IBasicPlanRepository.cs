using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBasicPlanRepository
    {
        Task<BasicPlan> AddAsync(BasicPlan entity);
        Task<BasicPlan> UpdateAsync(BasicPlan entity);
        Task<BasicPlan?> GetByIdAsync(Guid id);
        Task<List<BasicPlan>> GetAllAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}
