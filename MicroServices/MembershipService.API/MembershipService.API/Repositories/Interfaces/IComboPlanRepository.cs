using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IComboPlanRepository
    {
        Task<ComboPlan> AddAsync(ComboPlan entity);
        Task<ComboPlan> GetByIdAsync(Guid id);
        Task<List<ComboPlan>> GetAllAsync();
        Task<ComboPlan> UpdateAsync(ComboPlan entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
