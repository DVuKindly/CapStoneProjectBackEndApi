using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IComboPlanRepository
    {
        Task<ComboPlan> CreateAsync(ComboPlan comboPlan);
        Task<ComboPlan?> GetByIdAsync(Guid id);
        Task<List<ComboPlan>> GetAllAsync();
        Task UpdateAsync(ComboPlan comboPlan);
        Task DeleteAsync(ComboPlan comboPlan);
    }
}
