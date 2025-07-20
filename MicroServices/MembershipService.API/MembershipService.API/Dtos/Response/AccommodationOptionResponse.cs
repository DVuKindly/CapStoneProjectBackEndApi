namespace MembershipService.API.Dtos.Response
{
    public class AccommodationOptionResponse
    {
        public Guid Id { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public Guid? PropertyId { get; set; }
        public string? PropertyName { get; set; }
        public Guid NextUServiceId { get; set; }
        public string NextUServiceName { get; set; }

        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }
    }
}
