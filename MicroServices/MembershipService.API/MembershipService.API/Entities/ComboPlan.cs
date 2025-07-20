using MembershipService.API.Entities.Common;

namespace MembershipService.API.Entities
{
    public class ComboPlan : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsSuggested { get; set; }
        public bool VerifyBuy { get; set; }

        public Guid? PropertyId { get; set; }
        public Property? Property { get; set; } = null!;

        public Guid PackageLevelId { get; set; }
        public PackageLevel PackageLevel { get; set; }


        public ICollection<ComboPlanBasic> ComboPlanBasics { get; set; }
        public ICollection<Membership> Memberships { get; set; }
        public ICollection<MembershipHistory> MembershipHistories { get; set; }
        public ICollection<ComboPlanDuration> ComboPlanDurations { get; set; }

    }
}
