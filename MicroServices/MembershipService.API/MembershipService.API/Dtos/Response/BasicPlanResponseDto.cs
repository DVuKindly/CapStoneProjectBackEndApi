using MembershipService.API.Dtos.Request;
using MembershipService.API.Entities;

namespace MembershipService.API.Dtos.Response
{
    public class BasicPlanResponseDto
    {
        public Guid Id { get; set; }
        public string BasicPlanTypeCode { get; set; } // <-- Add this
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool VerifyBuy { get; set; }
        public Guid BasicPlanTypeId { get; set; }
        public string BasicPlanType { get; set; }
        public int PlanCategoryId { get; set; }
        public string PlanCategoryName { get; set; }
        public int PlanLevelId { get; set; }
        public string PlanLevelName { get; set; }
        public int TargetAudienceId { get; set; }
        public string TargetAudienceName { get; set; }

        public Guid? PropertyId { get; set; }
        public string? PropertyName { get; set; }
        public List<BasicPlanRoomResponseDto>? Acomodations { get; set; } = new();
        public List<EntitlementResponseDto>? Entitlements { get; set; } = new();
        public List<PlanDurationResponseDto> PlanDurations { get; set; } = new();

        //Vu 
        //public int PackageDurationValue { get; set; }
        //public string PackageDurationUnit { get; set; } = string.Empty;
        //public string DurationDescription { get; set; } = string.Empty;
        public string PlanSource { get; set; } = "basic";
    }

    public class PlanDurationResponseDto
    {
        public int PlanDurationId { get; set; }
        public decimal DiscountRate { get; set; }
        public string PlanDurationUnit { get; set; } = string.Empty;
        public string PlanDurationValue { get; set; } = string.Empty;
        public string PlanDurationDescription { get; set; } = string.Empty;
    }

    public class BasicPlanRoomResponseDto
    {
        public Guid AccomodationId { get; set; }
        public string AccomodationDescription { get; set; }
        public string RoomType { get; set; }
    }

    public class EntitlementResponseDto
    {
        public Guid EntitlementId { get; set; }

        public string NextUSerName { get; set; }
    }
}