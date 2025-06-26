using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class ComboPlanBasicRepository : IComboPlanBasicRepository
    {
        private readonly MembershipDbContext _context;

        public ComboPlanBasicRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<List<ComboPlanBasic>> GetByComboPlanIdAsync(Guid comboPlanId)
        {
            return await _context.ComboPlanBasics
                .Where(x => x.ComboPlanId == comboPlanId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<ComboPlanBasic> entities)
        {
            await _context.ComboPlanBasics.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByComboPlanIdAsync(Guid comboPlanId)
        {
            var toRemove = await _context.ComboPlanBasics
                .Where(x => x.ComboPlanId == comboPlanId)
                .ToListAsync();

            _context.ComboPlanBasics.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }
    }
}
