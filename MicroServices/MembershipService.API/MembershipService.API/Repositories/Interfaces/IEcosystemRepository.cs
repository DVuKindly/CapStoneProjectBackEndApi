using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IEcosystemRepository
    {
        Task<IEnumerable<Ecosystem>> GetAllAsync();
        Task<Ecosystem?> GetByIdAsync(Guid id);
        Task AddAsync(Ecosystem ecosystem);
        Task UpdateAsync(Ecosystem ecosystem);
        Task DeleteAsync(Ecosystem ecosystem);
        Task<bool> ExistsAsync(Guid id);

    }
}
