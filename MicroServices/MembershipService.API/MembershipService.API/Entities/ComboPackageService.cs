using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class ComboPackageService : AuditableEntity
    {
        public Guid Id { get; set; }

        public Guid ComboPackageId { get; set; }
        public ComboPackage ComboPackage { get; set; }

        public Guid NextUServiceId { get; set; }
        public NextUService NextUService { get; set; }
    }
}
