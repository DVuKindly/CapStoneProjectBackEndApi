using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface INextUServiceRepository
    {
        Task<NextUService> AddAsync(NextUService service);
        Task<NextUService> GetByIdAsync(Guid id);
        Task<List<NextUService>> GetAllAsync();
        Task<NextUService> UpdateAsync(NextUService service);
        Task<bool> DeleteAsync(Guid id);
        Task<Ecosystem> GetEcosystemByIdAsync(Guid id);
    }
}
