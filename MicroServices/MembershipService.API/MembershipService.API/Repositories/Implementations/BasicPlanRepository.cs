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

        public async Task<BasicPlan> AddAsync(BasicPlan entity)
        {
            _context.BasicPlans.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.BasicPlans.FindAsync(id);
            if (entity == null) return false;

            _context.BasicPlans.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BasicPlan>> GetAllAsync()
        {
            return await _context.BasicPlans
                .Include(x => x.ComboPlanDurations)
                .Include(x => x.Location)
                .Include(x => x.PlanCategory)
                .ToListAsync();
        }

        public async Task<BasicPlan> GetByIdAsync(Guid id)
        {
            return await _context.BasicPlans
                .Include(x => x.ComboPlanDurations)
                .Include(x => x.Location)
                .Include(x => x.PlanCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BasicPlan> UpdateAsync(BasicPlan entity)
        {
            _context.BasicPlans.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

    }
}
