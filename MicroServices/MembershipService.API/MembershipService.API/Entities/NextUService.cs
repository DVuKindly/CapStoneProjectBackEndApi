using MembershipService.API.Entities.Common;
using MembershipService.API.Enums;

namespace MembershipService.API.Entities
{
    public class NextUService : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ServiceType ServiceType { get; set; }

        public Guid EcosystemId { get; set; }
        public Ecosystem Ecosystem { get; set; }
        public Guid? LocationId { get; set; }
        public Location? Location { get; set; } = null!;

        public ICollection<Media> MediaGallery { get; set; }
        public ICollection<BasicPlanService> BasicPlanServices { get; set; }
        public ICollection<AccommodationOption> AccommodationOptions { get; set; }
        public ICollection<EntitlementRule> EntitlementRules { get; set; }

    }
}
