using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class PackageLevelRepository : IPackageLevelRepository
    {
        private readonly MembershipDbContext _context;

        public PackageLevelRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<List<PackageLevel>> GetAllAsync() =>
            await _context.PackageLevels.ToListAsync();

        public async Task<PackageLevel?> GetByIdAsync(Guid id) =>
            await _context.PackageLevels.FindAsync(id);

        public async Task<PackageLevel> CreateAsync(PackageLevel packageLevel)
        {
            _context.PackageLevels.Add(packageLevel);
            await _context.SaveChangesAsync();
            return packageLevel;
        }

        public async Task UpdateAsync(PackageLevel packageLevel)
        {
            _context.PackageLevels.Update(packageLevel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PackageLevel packageLevel)
        {
            _context.PackageLevels.Remove(packageLevel);
            await _context.SaveChangesAsync();
        }
    }
}
