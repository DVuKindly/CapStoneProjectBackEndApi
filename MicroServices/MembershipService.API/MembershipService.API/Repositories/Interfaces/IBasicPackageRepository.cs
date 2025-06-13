using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBasicPackageRepository
    {
        Task<BasicPackage> CreateAsync(BasicPackage package);
        Task<BasicPackage?> GetByIdAsync(Guid id);
        Task<List<BasicPackage>> GetAllAsync();
        Task UpdateAsync(BasicPackage package);
        Task DeleteAsync(BasicPackage package);
    }
}
