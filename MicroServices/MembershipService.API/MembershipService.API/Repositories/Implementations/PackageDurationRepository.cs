using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class PackageDurationRepository : IPackageDurationRepository
    {
        private readonly MembershipDbContext _context;

        public PackageDurationRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PackageDuration>> GetAllAsync()
        {
            return await _context.PackageDurations.ToListAsync();
        }

        public async Task<PackageDuration?> GetByIdAsync(int id)
        {
            return await _context.PackageDurations.FindAsync(id);
        }
    }
}
