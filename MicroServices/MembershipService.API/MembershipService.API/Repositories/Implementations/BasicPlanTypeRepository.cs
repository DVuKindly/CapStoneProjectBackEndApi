using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BasicPlanTypeRepository : IBasicPlanTypeRepository
    {
        private readonly MembershipDbContext _context;

        public BasicPlanTypeRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<List<BasicPlanType>> GetAllAsync()
        {
            return await _context.BasicPlanTypes.ToListAsync();
        }

        public async Task<BasicPlanType?> GetByIdAsync(Guid id)
        {
            return await _context.BasicPlanTypes.FindAsync(id);
        }
    }
}
