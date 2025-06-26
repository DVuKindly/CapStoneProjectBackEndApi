using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class AccommodationOption : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid NextUServiceId { get; set; }
        public NextUService NextUService { get; set; }

        public string RoomType { get; set; } = null!;
        public int QuantityAvailable { get; set; }
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public string? Description { get; set; }

        public ICollection<RoomInstance> Rooms { get; set; } = new List<RoomInstance>();
    }
}
