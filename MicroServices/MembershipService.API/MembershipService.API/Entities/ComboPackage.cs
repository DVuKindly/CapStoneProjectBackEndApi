using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class ComboPackage : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public float DiscountRate { get; set; }
        public bool IsSuggested { get; set; }

        public Guid BasicPackageId { get; set; }
        public BasicPackage BasicPackage { get; set; }

        public ICollection<ServicePricing> ServicePricings { get; set; }
        public ICollection<ComboPackageService> ComboPackageServices { get; set; }
    }
}
