using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class PlanLevelRepository : IPlanLevelRepository
    {
        private readonly MembershipDbContext _context;

        public PlanLevelRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlanLevel>> GetAllAsync()
        {
            return await _context.PlanLevels.ToListAsync();
        }
    }
}