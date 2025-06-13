using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class Ecosystem : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<NextUService> NextUServices { get; set; }
    }
}
