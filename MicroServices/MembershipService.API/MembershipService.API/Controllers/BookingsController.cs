using MembershipService.API.Dtos.Request;
using MembershipService.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var result = await _bookingService.CreateAsync(request);
            return Ok(result);
        }

        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetRoomBookings(Guid roomId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var result = await _bookingService.GetRoomBookingsAsync(roomId, from, to);
            return Ok(result);
        }
        [HttpGet("check")]
        public async Task<IActionResult> CheckRoomBooked([FromQuery] Guid roomId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var isBooked = await _bookingService.ValidateBookingAsync(roomId, startDate, endDate);
            return Ok(isBooked);
        }

        [HttpPost("hold")]
        public async Task<IActionResult> CreateHold([FromBody] CreateHoldBookingRequest request)
        {
            var success = await _bookingService.CreateHoldBookingAsync(request);
            return success ? Ok("Booking hold created.") : BadRequest("Hold booking failed.");
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmBooking([FromBody] ConfirmBookingRequest request)
        {
            var success = await _bookingService.ConfirmBookingAsync(request);
            return success ? Ok("Booking confirmed.") : BadRequest("Confirmation failed.");
        }

        [HttpPost("auto-cancel-expired")]
        public async Task<IActionResult> AutoCancelExpired()
        {
            var cancelled = await _bookingService.AutoCancelExpiredBookingsAsync();
            return Ok($"{cancelled} bookings auto-cancelled.");
        }
        [HttpPost("cancel-hold")]
        public async Task<IActionResult> CancelHold([FromBody] CancelHoldBookingRequest request)
        {
            var success = await _bookingService.CancelHoldBookingAsync(request);
            return success ? Ok("Hold booking cancelled.") : BadRequest("Cancellation failed.");
        }



    }
}
