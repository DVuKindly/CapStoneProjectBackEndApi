using MembershipService.API.Entities.Common;
using MembershipService.API.Enums;
using Microsoft.IdentityModel.Tokens;

namespace MembershipService.API.Entities
{
    public class PackageDuration : AuditableEntity
    {
        public int Id { get; set; }
        public int Value { get; set; }             // 1, 3, 12...
        public DurationUnit Unit { get; set; }     // Enum: Day, Week, Month, Year

        public string? Description { get; set; }

        public ICollection<BasicPackage> BasicPackages { get; set; }
    }
}
