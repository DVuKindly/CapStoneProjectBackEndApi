using MembershipService.API.Entities;

namespace MembershipService.API.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> AddAsync(Booking booking);
        Task<List<Booking>> GetRoomBookingsAsync(Guid roomInstanceId, DateTime? from, DateTime? to);
        //Task<IEnumerable<object>> GetRoomBookingsAsync(Guid roomInstanceId);
    }
}
