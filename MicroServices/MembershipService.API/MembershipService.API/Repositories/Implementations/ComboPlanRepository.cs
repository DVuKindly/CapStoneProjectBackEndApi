using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class ComboPlanRepository : IComboPlanRepository
    {
        private readonly MembershipDbContext _context;

        public ComboPlanRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<ComboPlan> AddAsync(ComboPlan entity)
        {
            _context.ComboPlans.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.ComboPlans.FindAsync(id);
            if (entity == null) return false;
            _context.ComboPlans.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ComboPlan>> GetAllAsync()
        {
            return await _context.ComboPlans
                .Include(x => x.ComboPlanBasics)
                .Include(x => x.ComboPlanDurations)
                .Include(x => x.Location)
                .Include(x => x.PackageLevel)
                .ToListAsync();
        }

        public async Task<ComboPlan> GetByIdAsync(Guid id)
        {
            return await _context.ComboPlans
                .Include(x => x.ComboPlanBasics)
                .Include(x => x.ComboPlanDurations)
                .Include(x => x.Location)
                .Include(x => x.PackageLevel)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ComboPlan> UpdateAsync(ComboPlan entity)
        {
            _context.ComboPlans.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

    }
}
