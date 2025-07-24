namespace UserService.API.DTOs.Requests
{
    public class HoldBookingRequestDto
    {
        public Guid MemberId { get; set; } 
        public Guid RoomInstanceId { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationValue { get; set; }
        public string DurationUnit { get; set; } = "day";

        public string PackageType { get; set; } = "basic";
    }

}
