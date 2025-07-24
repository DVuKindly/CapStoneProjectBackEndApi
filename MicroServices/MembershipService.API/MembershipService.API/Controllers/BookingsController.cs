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


    }
}
