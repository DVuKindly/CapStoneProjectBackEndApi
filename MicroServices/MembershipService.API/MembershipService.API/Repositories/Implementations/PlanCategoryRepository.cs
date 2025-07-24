using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class PlanCategoryRepository : IPlanCategoryRepository
    {
        private readonly MembershipDbContext _context;

        public PlanCategoryRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlanCategory>> GetAllAsync()
        {
            return await _context.PlanCategories.ToListAsync();
        }
    }
}