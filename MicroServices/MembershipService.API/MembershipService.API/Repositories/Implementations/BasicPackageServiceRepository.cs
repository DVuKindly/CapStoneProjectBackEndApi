using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BasicPackageServiceRepository : IBasicPackageServiceRepository
    {
        private readonly MembershipDbContext _context;

        public BasicPackageServiceRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<BasicPackageService> services)
        {
            await _context.BasicPackageServices.AddRangeAsync(services);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BasicPackageService>> GetByPackageIdAsync(Guid packageId)
        {
            return await _context.BasicPackageServices
                .Where(x => x.BasicPackageId == packageId)
                .ToListAsync();
        }

        public async Task DeleteByPackageIdAsync(Guid packageId)
        {
            var toRemove = await _context.BasicPackageServices
                .Where(x => x.BasicPackageId == packageId)
                .ToListAsync();

            _context.BasicPackageServices.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }
    }
}
