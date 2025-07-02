namespace MembershipService.API.Dtos.Response
{
    public class BasicPlanResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool VerifyBuy { get; set; }

        public Guid BasicPlanTypeId { get; set; }
        public string? BasicPlanTypeName { get; set; }

        public int BasicPlanCategoryId { get; set; }
        public string? BasicPlanCategoryName { get; set; }

        public int PlanLevelId { get; set; }
        public string? PlanLevelName { get; set; }

        public int TargetAudienceId { get; set; }
        public string? TargetAudienceName { get; set; }

        public Guid? LocationId { get; set; }
        public string? LocationName { get; set; }

        public List<BasicPlanRoomResponse> Rooms { get; set; } = new();

        public int PackageDurationId { get; set; }

        public string? DurationDescription { get; set; } 

        public List<Guid> NextUServiceIds { get; set; } = new();


      
        public int PackageDurationValue { get; set; }

       
        public string PackageDurationUnit { get; set; } = string.Empty;

      
        public string PlanSource { get; set; } = "basic";

    }
}
