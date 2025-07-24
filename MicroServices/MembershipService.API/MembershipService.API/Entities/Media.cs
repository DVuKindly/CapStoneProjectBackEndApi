using MembershipService.API.Entities.Common;
using MembershipService.API.Enums;

namespace MembershipService.API.Entities
{
    public class Media : AuditableEntity
    {
        public Guid Id { get; set; }

        public string Url { get; set; } = null!;
        public string Type { get; set; } = "image"; // "image", "video", etc.
        public string? Description { get; set; }

        // Generic reference
        public Guid ActorId { get; set; }
        public ActorType ActorType { get; set; }

    }

}