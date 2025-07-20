using MembershipService.API.Entities.Common;
using MembershipService.API.Enums;

namespace MembershipService.API.Entities
{
    public class RoomInstance : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid AccommodationOptionId { get; set; }
        public AccommodationOption AccommodationOption { get; set; }
        public decimal? AddOnFee { get; set; }

        public string RoomCode { get; set; } = null!;
        public string RoomName { get; set; }

        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public string? DescriptionDetails { get; set; }
        public Floor Floor { get; set; }

        //public ICollection<BasicPlanRoom> BasicPlanRooms { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        
    }
}
