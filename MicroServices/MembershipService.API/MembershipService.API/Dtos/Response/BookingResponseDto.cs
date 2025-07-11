namespace MembershipService.API.Dtos.Response
{
    public class BookingResponseDto
    {
        public Guid? BookingId { get; set; }
        public Guid RoomInstanceId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public string ViewedBookingStatus { get; set; } 

    }
}
