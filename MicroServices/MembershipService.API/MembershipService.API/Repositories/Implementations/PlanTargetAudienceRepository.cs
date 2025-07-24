using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class PlanTargetAudienceRepository : IPlanTargetAudienceRepository
    {
        private readonly MembershipDbContext _context;

        public PlanTargetAudienceRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<List<PlanTargetAudience>> GetAllAsync()
        {
            return await _context.PlanTargetAudiences.ToListAsync();
        }
    }
}