using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class Media : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Url { get; set; }           // Link ảnh/video
        public string Type { get; set; }          // "image", "video"
        public string? Description { get; set; }

        public Guid? NextUServiceId { get; set; }
        public NextUService? NextUService { get; set; }

        
    }

}
