namespace MembershipService.API.Dtos.Response
{
    public class BasicPlanRoomResponse
    {
        public Guid RoomId { get; set; }
        public string? RoomName { get; set; }
        public int NightsIncluded { get; set; }
        public decimal? CustomPricePerNight { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
