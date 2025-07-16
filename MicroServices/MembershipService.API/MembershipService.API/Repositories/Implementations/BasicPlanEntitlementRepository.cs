using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BasicPlanEntitlementRepository : IBasicPlanEntitlementRepository
    {
        private readonly MembershipDbContext _context;

        public BasicPlanEntitlementRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<BasicPlanEntitlement?> AddAsync(BasicPlanEntitlement entity)
        {
            _context.BasicPlanEntitlements.Add(entity);
            var result = await _context.SaveChangesAsync();
            return result > 0 ? entity : null;
        }

        public async Task<IEnumerable<BasicPlanEntitlement>> AddRangeAsync(IEnumerable<BasicPlanEntitlement> entities)
        {
            _context.BasicPlanEntitlements.AddRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task<IEnumerable<BasicPlanEntitlement>> GetByBasicPlanIdAsync(Guid basicPlanId)
        {
            return await _context.BasicPlanEntitlements
                .Include(x => x.EntitlementRule)
                .Where(x => x.BasicPlanId == basicPlanId)
                .ToListAsync();
        }

        public async Task DeleteByBasicPlanIdAsync(Guid basicPlanId)
        {
            var items = await _context.BasicPlanEntitlements
                .Where(x => x.BasicPlanId == basicPlanId)
                .ToListAsync();

            _context.BasicPlanEntitlements.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
