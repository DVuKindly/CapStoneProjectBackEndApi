using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class ComboPlanServiceRepository : IComboPlanServiceRepository
    {
        private readonly MembershipDbContext _context;

        public ComboPlanServiceRepository(MembershipDbContext context)
        {
            _context = context;
        }
        public async Task AddRangeAsync(IEnumerable<ComboPlanService> services)
        {
            await _context.ComboPlanServices.AddRangeAsync(services);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ComboPlanService>> GetByPackageIdAsync(Guid comboPlanId)
        {
            return await _context.ComboPlanServices
                .Where(x => x.ComboPlanId == comboPlanId)
                .ToListAsync();
        }
        public async Task DeleteByPackageIdAsync(Guid comboPlanId)
        {
            var toRemove = await _context.ComboPlanServices
                .Where(x => x.ComboPlanId == comboPlanId)
                .ToListAsync();

            _context.ComboPlanServices.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }


    }
}
