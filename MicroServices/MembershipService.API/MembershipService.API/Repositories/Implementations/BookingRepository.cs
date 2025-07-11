using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MembershipDbContext _context;

        public BookingRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<List<Booking>> GetRoomBookingsAsync(Guid roomInstanceId, DateTime from, DateTime to)
        {
            return await _context.Bookings
                .Where(b => b.RoomInstanceId == roomInstanceId &&
                            b.StartDate <= to && b.EndDate >=  from && b.Status == Enums.BookingStatus.Confirmed)
                .ToListAsync();
        }
    }
}

