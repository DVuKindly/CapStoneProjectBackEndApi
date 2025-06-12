using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class EcosystemRepository : IEcosystemRepository
    {
        private readonly MembershipDbContext _context;

        public EcosystemRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ecosystem>> GetAllAsync()
        {
            return await _context.Ecosystems.Include(x => x.NextUServices).ToListAsync();
        }

        public async Task<Ecosystem?> GetByIdAsync(Guid id)
        {
            return await _context.Ecosystems
                                 .Include(x => x.NextUServices)
                                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Ecosystem ecosystem)
        {
            await _context.Ecosystems.AddAsync(ecosystem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ecosystem ecosystem)
        {
            _context.Ecosystems.Update(ecosystem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Ecosystem ecosystem)
        {
            _context.Ecosystems.Remove(ecosystem);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Ecosystems.AnyAsync(e => e.Id == id);
        }

    }
}
