using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BasicPackageRepository : IBasicPackageRepository
    {
        private readonly MembershipDbContext _context;

        public BasicPackageRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<BasicPackage> CreateAsync(BasicPackage package)
        {
            _context.BasicPackages.Add(package);
            await _context.SaveChangesAsync();
            return package;
        }

        public async Task<BasicPackage?> GetByIdAsync(Guid id)
        {
            return await _context.BasicPackages
                .Include(x => x.BasicPackageServices)
                .Include(x => x.PackageLevel)
                .Include(x => x.PackageDuration)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<BasicPackage>> GetAllAsync()
        {
            return await _context.BasicPackages
                .Include(x => x.BasicPackageServices)
                .Include(x => x.PackageLevel)
                .Include(x => x.PackageDuration)
                .ToListAsync();
        }

        public async Task UpdateAsync(BasicPackage package)
        {
            _context.BasicPackages.Update(package);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BasicPackage package)
        {
            _context.BasicPackages.Remove(package);
            await _context.SaveChangesAsync();
        }

    }
}
