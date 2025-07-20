using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Enums;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MembershipService.API.Repositories.Implementations
{
    public class RoomInstanceRepository : IRoomInstanceRepository
    {
        private readonly MembershipDbContext _context;

        public RoomInstanceRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<RoomInstance> GetByIdWithNavigationAsync(Guid id)
        {
            return await _context.Rooms
                .Include(r => r.RoomViewOption)
                .Include(r => r.RoomFloorOption)
                .Include(r => r.BedTypeOption)
                .Include(r => r.RoomSizeOption)
                .Include(r => r.AccommodationOption)
                    .ThenInclude(a => a.Property)
                       .ThenInclude(b => b.Location)
                           .ThenInclude(c => c.City)
                .Include(r => r.AccommodationOption)
                    .ThenInclude(opt => opt.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);
        }


        //  Hàm dùng chung Include đầy đủ các liên kết cần thiết cho response
        private IQueryable<RoomInstance> GetRoomWithNavigation()
        {
            return _context.Rooms
                .Include(r => r.AccommodationOption)
                    .ThenInclude(opt => opt.RoomType)
                .Include(r => r.AccommodationOption)
                    .ThenInclude(opt => opt.Property)
                       .ThenInclude(b => b.Location)
                           .ThenInclude(c => c.City)
                .Include(r => r.RoomSizeOption)
                .Include(r => r.RoomViewOption)
                .Include(r => r.RoomFloorOption)
                .Include(r => r.BedTypeOption);
        }

        //  Get All
        public async Task<List<RoomInstance>> GetAllAsync()
        {
            return await GetRoomWithNavigation().ToListAsync();
        }

        //  Get theo Id
        public async Task<RoomInstance?> GetByIdAsync(Guid id)
        {
            return await GetRoomWithNavigation()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        //  Get theo AccommodationOptionId
        public async Task<List<RoomInstance>> GetByAccommodationOptionIdAsync(Guid optionId)
        {
            return await GetRoomWithNavigation()
                .Where(r => r.AccommodationOptionId == optionId)
                .ToListAsync();
        }

        //  Get theo PropertyId (lọc qua AccommodationOption.PropertyId)
        public async Task<List<RoomInstance>> GetByPropertyIdAsync(Guid propertyId)
        {
            return await GetRoomWithNavigation()
                .Where(r => r.AccommodationOption.PropertyId == propertyId)
                .ToListAsync();
        }



        public async Task<RoomInstance> CreateAsync(RoomInstance entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Rooms.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> IsRoomCodeDuplicateAsync(Guid accommodationOptionId, string roomCode)
        {
            return await _context.Rooms
                .AnyAsync(r => r.AccommodationOptionId == accommodationOptionId && r.RoomCode == roomCode);
        }


        public async Task<RoomInstance> UpdateAsync(RoomInstance entity)
        {
            var existing = await _context.Rooms.FindAsync(entity.Id);
            if (existing == null) throw new Exception("RoomInstance not found");

            existing.RoomCode = entity.RoomCode;
            existing.RoomName = entity.RoomName;
            existing.DescriptionDetails = entity.DescriptionDetails;
            existing.Status = entity.Status;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Rooms.FindAsync(id);
            if (entity == null) return false;

            _context.Rooms.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
