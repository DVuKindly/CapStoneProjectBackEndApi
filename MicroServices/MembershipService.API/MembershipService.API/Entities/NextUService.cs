using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class NextUService : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitType { get; set; }
        public decimal Price { get; set; }

        public Guid EcosystemId { get; set; }
        public Ecosystem Ecosystem { get; set; }
        public Guid? LocationId { get; set; }
        public Location? Location { get; set; } = null!;

        public ICollection<ComboPlanService> ComboPlanServices { get; set; }
        public ICollection<Media> MediaGallery { get; set; }
        public ICollection<BasicPlanService> BasicPlanServices { get; set; }
    }
}
