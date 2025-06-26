using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class ComboPlanDurationRepository : IComboPlanDurationRepository
    {
        private readonly MembershipDbContext _context;

        public ComboPlanDurationRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<List<ComboPlanDuration>> GetByBasicPlanIdAsync(Guid basicPlanId)
        {
            return await _context.ComboPlanDurations
                .Where(x => x.BasicPlanId == basicPlanId)
                .ToListAsync();
        }
        public async Task<List<ComboPlanDuration>> GetByComboPlanIdAsync(Guid comboPlanId)
        {
            return await _context.ComboPlanDurations
                .Where(x => x.ComboPlanId == comboPlanId)
                .ToListAsync();
        }


        public async Task AddRangeAsync(List<ComboPlanDuration> durations)
        {
            await _context.ComboPlanDurations.AddRangeAsync(durations);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveByBasicPlanIdAsync(Guid basicPlanId)
        {
            var existing = await _context.ComboPlanDurations
                .Where(x => x.BasicPlanId == basicPlanId)
                .ToListAsync();

            _context.ComboPlanDurations.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveByComboPlanIdAsync(Guid comboPlanId)
        {
            var existing = await _context.ComboPlanDurations
                .Where(x => x.ComboPlanId == comboPlanId)
                .ToListAsync();

            _context.ComboPlanDurations.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }



    }
}
