using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;

namespace MembershipService.API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseDto> CreateAsync(CreateBookingRequest request);
        Task<List<BookingResponseDto>> GetRoomBookingsAsync(Guid roomInstanceId, DateTime? from, DateTime? to);
        Task<bool> ValidateBookingAsync(Guid roomInstanceId, DateTime startDate, DateTime endDate);

        Task<bool> CreateHoldBookingAsync(CreateHoldBookingRequest request);
        Task<bool> ConfirmBookingAsync(ConfirmBookingRequest request);
        Task<int> AutoCancelExpiredBookingsAsync();
        Task<bool> CancelHoldBookingAsync(CancelHoldBookingRequest request);

    }
}
