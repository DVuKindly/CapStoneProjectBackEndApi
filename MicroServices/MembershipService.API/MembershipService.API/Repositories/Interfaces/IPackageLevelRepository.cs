using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IPackageLevelRepository
    {
        Task<List<PackageLevel>> GetAllAsync();
        Task<PackageLevel?> GetByIdAsync(Guid id);
        Task<PackageLevel> CreateAsync(PackageLevel packageLevel);
        Task UpdateAsync(PackageLevel packageLevel);
        Task DeleteAsync(PackageLevel packageLevel);
    }
}
