using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class NextUServiceRepository : INextUServiceRepository
    {
        private readonly MembershipDbContext _context;

        public NextUServiceRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<NextUService> AddAsync(NextUService service)
        {
            _context.NextUServices.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<NextUService> GetByIdAsync(Guid id)
        {
            return await _context.NextUServices
                .Include(x => x.Ecosystem)
                .Include(x => x.Location)
                .Include(x => x.MediaGallery)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<NextUService>> GetAllAsync()
        {
            return await _context.NextUServices
                .Include(x => x.Ecosystem)
                .Include(x => x.Location)
                .Include(x => x.MediaGallery)
                .ToListAsync();
        }

        public async Task<NextUService> UpdateAsync(NextUService service)
        {
            _context.NextUServices.Update(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.NextUServices.FindAsync(id);
            if (entity == null) return false;
            _context.NextUServices.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Ecosystem> GetEcosystemByIdAsync(Guid id)
        {
            return await _context.Ecosystems.FindAsync(id);
        }
    }
}
