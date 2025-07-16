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

        

        public async Task<List<RoomInstance>> GetByAccommodationOptionIdAsync(Guid optionId)
        {
            return await _context.Rooms
                .Include(r => r.AccommodationOption)
                    .ThenInclude(opt => opt.RoomType)
                .Include(r => r.AccommodationOption)
                     .ThenInclude(opt => opt.Location)
                .Where(r => r.AccommodationOptionId == optionId)
                .ToListAsync();
        }

        public async Task<List<RoomInstance>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.AccommodationOption)
                    .ThenInclude(opt => opt.RoomType)
                .Include(r => r.AccommodationOption)
                     .ThenInclude(opt => opt.Location)
                .ToListAsync();
        }

        //public async Task<List<RoomInstance>> GetByBasicPlanIdAsync(Guid planId)
        //{
        //    var roomIds = await _context.BasicPlanRooms
        //        .Where(bpr => bpr.BasicPlanId == planId)
        //        .Select(bpr => bpr.RoomInstanceId)
        //        .ToListAsync();

        //    return await _context.Rooms
        //        .Where(r => roomIds.Contains(r.Id))
        //        .Include(r => r.AccommodationOption)
        //            .ThenInclude(opt => opt.RoomType)
        //        .Include(r => r.AccommodationOption)
        //            .ThenInclude(opt => opt.Location)
        //        .ToListAsync();
        //}

        public async Task<List<RoomInstance>> GetByLocationIdAsync(Guid locationId)
        {
            
            return await _context.Rooms
                .Include(r => r.AccommodationOption)
                    .ThenInclude(opt => opt.RoomType)
                .Include(r => r.AccommodationOption)
                   .ThenInclude(opt => opt.Location)
                .Where(r => r.AccommodationOption.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<RoomInstance?> GetByIdAsync(Guid id)
        {
            return await _context.Rooms
                .Include(r => r.AccommodationOption)
                    .ThenInclude(opt => opt.RoomType)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        

        public async Task<RoomInstance> CreateAsync(RoomInstance entity)
        {
            entity.Id = Guid.NewGuid();
            _context.Rooms.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<RoomInstance> UpdateAsync(RoomInstance entity)
        {
            var existing = await _context.Rooms.FindAsync(entity.Id);
            if (existing == null) throw new Exception("RoomInstance not found");

            existing.RoomCode = entity.RoomCode;
            existing.RoomName = entity.RoomName;
            existing.DescriptionDetails = entity.DescriptionDetails;
            existing.Floor = entity.Floor;
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
