namespace MembershipService.API.Dtos.Request
{
    public class CreateAccommodationOptionRequest
    {
        public int RoomTypeId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? NextUServiceId { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateAccommodationOptionRequest : CreateAccommodationOptionRequest
    {
    }
}
