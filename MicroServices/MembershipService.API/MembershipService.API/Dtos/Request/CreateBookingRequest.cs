namespace MembershipService.API.Dtos.Request
{
    public class CreateBookingRequest
    {
        public Guid MemberId { get; set; }
        public Guid RoomInstanceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Note { get; set; }
    }
}
