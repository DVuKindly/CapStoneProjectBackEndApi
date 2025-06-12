using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class BasicPackage : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Duration { get; set; } // in days
        public decimal Price { get; set; }
        public bool VerifyBuy { get; set; }

        public Guid PackageTypeId { get; set; }
        public PackageType PackageType { get; set; }

        public Guid? PackageLevelId { get; set; }
        public PackageLevel? PackageLevel { get; set; }
        public int PackageDurationId { get; set; }       
        public PackageDuration PackageDuration { get; set; }

        public ICollection<Media> MediaGallery { get; set; }
        public ICollection<ServicePricing> ServicePricings { get; set; }
        public ICollection<ComboPackage> ComboPackages { get; set; }
        public ICollection<BasicPackageService> BasicPackageServices { get; set; }
    }
}
