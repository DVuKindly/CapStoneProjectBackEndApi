namespace UserService.API.DTOs.Requests
{
    public class ConfirmBookingRequestDto
    {
        public Guid MemberId { get; set; }
        public Guid RoomInstanceId { get; set; }
        public DateTime StartDate { get; set; }
    }

}
