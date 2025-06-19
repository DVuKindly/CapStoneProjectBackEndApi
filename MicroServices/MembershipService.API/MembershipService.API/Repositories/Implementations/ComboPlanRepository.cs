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

        public async Task<ComboPlan> CreateAsync(ComboPlan comboPlan)
        {
            _context.ComboPlans.Add(comboPlan);
            await _context.SaveChangesAsync();
            return comboPlan;
        }

        public async Task<ComboPlan?> GetByIdAsync(Guid id)
        {
            return await _context.ComboPlans
                .Include(x => x.ComboPlanServices)
                .Include(x => x.PackageLevel)
                .Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<ComboPlan>> GetAllAsync()
        {
            return await  _context.ComboPlans
                .Include(x => x.ComboPlanServices)
                .Include(x => x.PackageLevel)
                .Include(x => x.Location)
                .ToListAsync(); 
        }

        public async Task UpdateAsync(ComboPlan comboPlan)
        {
            _context.ComboPlans.Update(comboPlan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ComboPlan comboPlan)
        {
            _context.ComboPlans.Remove(comboPlan);
            await _context.SaveChangesAsync();
        }

    }
}
