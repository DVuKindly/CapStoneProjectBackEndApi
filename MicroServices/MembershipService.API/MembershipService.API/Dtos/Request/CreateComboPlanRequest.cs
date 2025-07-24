using System.ComponentModel.DataAnnotations;

namespace MembershipService.API.Dtos.Request
{
    public class CreateComboPlanRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsSuggested { get; set; }
        public bool VerifyBuy { get; set; }
        public Guid? PropertyId { get; set; }
        [Required]
        public int BasicPlanCategoryId { get; set; }

        [Required]
        public int PlanLevelId { get; set; }

        [Required]
        public int TargetAudienceId { get; set; }

        public List<Guid> BasicPlanIds { get; set; } = new();
        public List<PackageDurationDto> PackageDurations { get; set; } = new();
    }

    public class UpdateComboPlanRequest : CreateComboPlanRequest { }
}