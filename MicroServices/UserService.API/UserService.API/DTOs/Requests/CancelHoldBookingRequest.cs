namespace UserService.API.DTOs.Requests
{
    public class CancelHoldBookingRequest
    {
        public Guid MemberId { get; set; }
        public Guid RoomInstanceId { get; set; }
        public DateTime StartDate { get; set; }
    }

}
