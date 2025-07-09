using MembershipService.API.Dtos.Request;
using MembershipService.API.Entities;

namespace MembershipService.API.Dtos.Response
{
    public class BasicPlanResponseDto
    {
        public Guid Id { get; set; }
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

        public Guid? LocationId { get; set; }
        public string? LocationName { get; set; }
        public List<PlanDurationResponseDto> PlanDurations { get; set; } = new();
        public List<BasicPlanRoomResponseDto> Rooms { get; set; } = new();

        //Vu 
        public int PackageDurationValue { get; set; }
        public string PackageDurationUnit { get; set; } = string.Empty;
        public string DurationDescription { get; set; } = string.Empty;
        public string PlanSource { get; set; } = "basic";
    }

    public class BasicPlanRoomResponseDto
    {
        public Guid RoomInstanceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NightsIncluded { get; set; }
        public decimal? CustomPricePerNight { get; set; }
        public decimal? TotalPrice { get; set; }
    }

    public class PlanDurationResponseDto
    {
        public int PlanDurationId { get; set; }
        public decimal DiscountRate { get; set; }
        public string PlanDurationUnit { get; set; } = string.Empty;
        public string PlanDurationValue { get; set; } = string.Empty;
        public string PlanDurationDescription { get; set; } = string.Empty;
    }

}