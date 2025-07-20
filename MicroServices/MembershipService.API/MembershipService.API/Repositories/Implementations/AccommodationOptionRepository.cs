using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class AccommodationOptionRepository : IAccommodationOptionRepository
    {
        private readonly MembershipDbContext _context;

        public AccommodationOptionRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<List<AccommodationOption>> GetAllAsync()
        {
            return await _context.AccommodationOptions
                .Include(a => a.RoomType)
                .Include(a => a.Property)
                .Include(a => a.NextUService)
                .ToListAsync();
        }

        public async Task<AccommodationOption?> GetByIdAsync(Guid id)
        {
            return await _context.AccommodationOptions
                .Include(a => a.RoomType)
                .Include(a => a.Property)
                .Include(a => a.NextUService)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AccommodationOption> CreateAsync(AccommodationOption option)
        {
            option.Id = Guid.NewGuid();
            _context.AccommodationOptions.Add(option);
            await _context.SaveChangesAsync();
            return option;
        }

        public async Task<AccommodationOption> UpdateAsync(AccommodationOption option)
        {
            var existing = await _context.AccommodationOptions.FindAsync(option.Id);
            if (existing == null) throw new Exception("AccommodationOption not found");

            existing.RoomTypeId = option.RoomTypeId;
            existing.PropertyId = option.PropertyId;
            existing.Capacity = option.Capacity;
            existing.PricePerNight = option.PricePerNight;
            existing.Description = option.Description;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.AccommodationOptions.FindAsync(id);
            if (entity == null) return false;

            _context.AccommodationOptions.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
