namespace MembershipService.API.Dtos.Request
{
    public class CreateHoldBookingRequest
    {
        public Guid MemberId { get; set; }
        public Guid RoomInstanceId { get; set; }
        public DateTime StartDate { get; set; }
        public int DurationValue { get; set; }
        public string DurationUnit { get; set; }
        public string PackageType { get; set; } = "basic";
    }

}
