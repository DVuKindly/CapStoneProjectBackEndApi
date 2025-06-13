using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IPackageDurationRepository
    {
        Task<IEnumerable<PackageDuration>> GetAllAsync();
        Task<PackageDuration?> GetByIdAsync(int id);
    }
}
