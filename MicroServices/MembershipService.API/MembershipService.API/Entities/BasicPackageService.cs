using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class BasicPackageService : AuditableEntity
    {
        public Guid Id { get; set; }

        // FK đến BasicPackage
        public Guid BasicPackageId { get; set; }
        public BasicPackage BasicPackage { get; set; }

        // FK đến dịch vụ
        public Guid NextUServiceId { get; set; }
        public NextUService NextUService { get; set; }
    }
}
