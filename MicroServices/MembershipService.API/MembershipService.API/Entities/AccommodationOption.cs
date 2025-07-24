using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class AccommodationOption : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid NextUServiceId { get; set; }
        public NextUService NextUService { get; set; }

        public Guid? PropertyId { get; set; }
        public Property? Property { get; set; } = null!;

        public int RoomTypeId { get; set; }
        public RoomType? RoomType { get; set; } = null!;
        public int Capacity { get; set; }
        public decimal PricePerNight { get; set; }
        public string Description { get; set; }

        public ICollection<RoomInstance> Rooms { get; set; } = new List<RoomInstance>();
        public ICollection<BasicPlanRoom> BasicPlanRooms { get; set; } = new List<BasicPlanRoom>();


    }
}