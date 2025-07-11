using MembershipService.API.Data;
using MembershipService.API.Entities;
using MembershipService.API.Enums;
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

        public async Task<List<Booking>> GetRoomBookingsAsync(Guid roomInstanceId, DateTime? from, DateTime? to)
        {
            var actualFrom = from ?? DateTime.MinValue;
            var actualTo = to ?? DateTime.MaxValue;

            return await _context.Bookings
                .Where(b => b.RoomInstanceId == roomInstanceId &&
                            b.Status == BookingStatus.Confirmed &&
                            b.StartDate <= actualTo && b.EndDate >= actualFrom)
                .ToListAsync();
        }



        //public async Task<List<Booking>> GetAllConfirmedBookingsForRoomAsync(Guid roomInstanceId)
        //{
        //    return await _context.Bookings
        //        .Where(b => b.RoomInstanceId == roomInstanceId && b.Status == BookingStatus.Confirmed)
        //        .OrderBy(b => b.StartDate)
        //        .ToListAsync();
        //}

    }
}

