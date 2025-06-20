using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BasicPlanServiceRepository : IBasicPlanServiceRepository
    {
        private readonly MembershipDbContext _context;

        public BasicPlanServiceRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<BasicPlanService> services)
        {
            await _context.BasicPlanServices.AddRangeAsync(services);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BasicPlanService>> GetByPackageIdAsync(Guid packageId)
        {
            return await _context.BasicPlanServices
                .Where(x => x.BasicPlanId == packageId)
                .ToListAsync();
        }

        public async Task DeleteByPackageIdAsync(Guid packageId)
        {
            var toRemove = await _context.BasicPlanServices
                .Where(x => x.BasicPlanId == packageId)
                .ToListAsync();

            _context.BasicPlanServices.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }
    }
}
