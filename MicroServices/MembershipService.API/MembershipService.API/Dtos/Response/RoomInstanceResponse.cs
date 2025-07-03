namespace MembershipService.API.Dtos.Response
{
    public class RoomInstanceResponse
    {
        public Guid Id { get; set; }
        public Guid AccommodationOptionId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public string DescriptionDetails { get; set; }
        public string Status { get; set; }
        public string Floor { get; set; }
        public string RoomTypeName { get; set; }
    }
}
