using MembershipService.API.Dtos.Request;
using MembershipService.API.Entities;

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
    public List<Guid> ServiceIds { get; set; } = new();
    public List<PackageDurationDto> PackageDurations { get; set; } = new();

  
   public int PackageDurationValue { get; set; }
   public string PackageDurationUnit { get; set; } = string.Empty;
   public string DurationDescription { get; set; } = string.Empty;
   public string PlanSource { get; set; } = "basic";
}
