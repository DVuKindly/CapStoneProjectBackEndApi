using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class ServicePricing : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid NextUServiceId { get; set; }
        public NextUService NextUService { get; set; }

        public Guid? ComboPackageId { get; set; }
        public ComboPackage? ComboPackage { get; set; }

        public Guid? BasicPackageId { get; set; }
        public BasicPackage? BasicPackage { get; set; }

        public decimal CreditCost { get; set; }
        public decimal? OverridePrice { get; set; }
        public bool IsOptional { get; set; }
        public bool IsDefault { get; set; }
    }
}
