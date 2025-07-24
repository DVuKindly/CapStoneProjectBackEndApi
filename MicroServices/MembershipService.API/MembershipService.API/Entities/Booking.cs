using MembershipService.API.Entities.Common;
using MembershipService.API.Enums;

namespace MembershipService.API.Entities
{
    public class Booking : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid MemberId { get; set; }

        public Guid RoomInstanceId { get; set; }
        public RoomInstance RoomInstance { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;

        public DateTime? ExpiredAt { get; set; }
        public string? Note { get; set; }

    }
}