using System.ComponentModel.DataAnnotations;

namespace MembershipService.API.Dtos.Request
{
    public class CreateBasicPlanRequest
    {
        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; } // ✅ Bổ sung mô tả

        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public decimal Price { get; set; }

        public bool VerifyBuy { get; set; } = false; // ✅ Mặc định false
        public bool RequireBooking { get; set; } = false; // ✅ Mặc định false

        [Required]
        public Guid BasicPlanTypeId { get; set; }

        [Required]
        public int BasicPlanCategoryId { get; set; }

        [Required]
        public int PlanLevelId { get; set; }

        [Required]
        public int TargetAudienceId { get; set; }

        [Required]
        public Guid PropertyId { get; set; }

        public List<BasicPlanRoomDto>? Accomodations { get; set; } = new();
        public List<PlanEntitlementDto>? Entitlements { get; set; } = new();

        [MinLength(1, ErrorMessage = "At least one duration is required.")]
        public List<PackageDurationDto> PackageDurations { get; set; } = new();
    }


    public class BasicPlanRoomDto
    {
        public Guid AccomodationId { get; set; }
    }

    public class PlanEntitlementDto
    {
        public Guid EntitlementRuleId { get; set; }
        public int? Quantity { get; set; }
    }

    public class PackageDurationDto
    {
        public int DurationId { get; set; }
        public decimal DiscountRate { get; set; }
    }

}
