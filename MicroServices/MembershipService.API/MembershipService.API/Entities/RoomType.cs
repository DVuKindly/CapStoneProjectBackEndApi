using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class RoomType : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<AccommodationOption> AccommodationOptions { get; set; }

    }
}
