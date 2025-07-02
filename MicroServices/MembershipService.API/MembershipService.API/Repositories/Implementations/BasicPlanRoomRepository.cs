using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BasicPlanRoomRepository : IBasicPlanRoomRepository
    {
        private readonly MembershipDbContext _context;

        public BasicPlanRoomRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<BasicPlanRoom> AddAsync(BasicPlanRoom room)
        {
            _context.BasicPlanRooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<BasicPlanRoom> UpdateAsync(BasicPlanRoom room)
        {
            var existing = await _context.BasicPlanRooms.FindAsync(room.Id);
            if (existing == null) throw new Exception("BasicPlanRoom not found");

            existing.NightsIncluded = room.NightsIncluded;
            existing.CustomPricePerNight = room.CustomPricePerNight;
            existing.TotalPrice = room.TotalPrice;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var room = await _context.BasicPlanRooms.FindAsync(id);
            if (room == null) return false;

            _context.BasicPlanRooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BasicPlanRoom>> GetByBasicPlanIdAsync(Guid basicPlanId)
        {
            return await _context.BasicPlanRooms
                .Include(r => r.RoomInstance)
                .Where(r => r.BasicPlanId == basicPlanId)
                .ToListAsync();
        }

        public async Task<BasicPlanRoom?> GetByIdAsync(Guid id)
        {
            return await _context.BasicPlanRooms
                .Include(r => r.RoomInstance)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> ExistsAsync(Guid basicPlanId, Guid roomId)
        {
            return await _context.BasicPlanRooms
                .AnyAsync(r => r.BasicPlanId == basicPlanId && r.RoomId == roomId);
        }
    }
}
