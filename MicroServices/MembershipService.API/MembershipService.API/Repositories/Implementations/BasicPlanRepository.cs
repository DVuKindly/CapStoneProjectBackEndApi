using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BasicPlanRepository : IBasicPlanRepository
    {
        private readonly MembershipDbContext _context;

        public BasicPlanRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<BasicPlan> CreateAsync(BasicPlan package)
        {
            _context.BasicPlans.Add(package);
            await _context.SaveChangesAsync();
            return package;
        }

        public async Task<BasicPlan?> GetByIdAsync(Guid id)
        {
            return await _context.BasicPlans
                .Include(x => x.BasicPlanServices)
                .Include(x => x.PackageDuration)
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<BasicPlan>> GetAllAsync()
        {
            return await _context.BasicPlans
                .Include(x => x.BasicPlanServices)
                .Include(x => x.PackageDuration)
                .Include(x => x.Location)
                .ToListAsync();
        }

        public async Task UpdateAsync(BasicPlan package)
        {
            _context.BasicPlans.Update(package);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(BasicPlan package)
        {
            _context.BasicPlans.Remove(package);
            await _context.SaveChangesAsync();
        }

    }
}
