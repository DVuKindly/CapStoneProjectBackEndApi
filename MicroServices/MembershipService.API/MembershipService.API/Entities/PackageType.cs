using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class PackageType : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<BasicPackage> BasicPackages { get; set; }
    }
}
