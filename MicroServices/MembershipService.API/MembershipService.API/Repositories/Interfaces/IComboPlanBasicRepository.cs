using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IComboPlanBasicRepository
    {
        Task<List<ComboPlanBasic>> GetByComboPlanIdAsync(Guid comboPlanId);
        Task AddRangeAsync(IEnumerable<ComboPlanBasic> entities);
        Task DeleteByComboPlanIdAsync(Guid comboPlanId);
    }
}
