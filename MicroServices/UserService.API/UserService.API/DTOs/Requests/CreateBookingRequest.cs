namespace UserService.API.DTOs.Requests
{
    public class CreateBookingRequest
    {
        public Guid MemberId { get; set; }             // AccountId bên UserService
        public Guid RoomInstanceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Note { get; set; }
    }

}
