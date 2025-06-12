using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class NextUService : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitType { get; set; }

        public Guid EcosystemId { get; set; }
        public Ecosystem Ecosystem { get; set; }


        public ICollection<ServicePricing> ServicePricings { get; set; }
        public ICollection<ComboPackageService> ComboPackageServices { get; set; }
        public ICollection<Media> MediaGallery { get; set; }
        public ICollection<BasicPackageService> BasicPackageServices { get; set; }
    }
}
