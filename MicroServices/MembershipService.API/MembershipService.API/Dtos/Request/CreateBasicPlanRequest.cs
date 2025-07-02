namespace MembershipService.API.Dtos.Request
{
    public class CreateBasicPlanRequest
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool VerifyBuy { get; set; }

        public Guid BasicPlanTypeId { get; set; }
        public int BasicPlanCategoryId { get; set; }
        public int PlanLevelId { get; set; }
        public int TargetAudienceId { get; set; }
        public Guid? LocationId { get; set; }

        public List<BasicPlanRoomDto>? Rooms { get; set; } = new();
        public List<PackageDurationDto> PackageDurations { get; set; } = new();
    }

    public class BasicPlanRoomDto
    {
        public Guid RoomId { get; set; }
        public int NightsIncluded { get; set; }
        public decimal? CustomPricePerNight { get; set; }
    }
    public class PackageDurationDto
    {
        public int DurationId { get; set; }
        public decimal DiscountRate { get; set; }
    }

    public class CalculateBasicPlanPriceRequest
    {
        public List<RoomPriceItem> Rooms { get; set; } = new();
    }

    public class RoomPriceItem
    {
        public Guid RoomId { get; set; }
        public int Nights { get; set; }
        public decimal? CustomPricePerNight { get; set; }
    }
}
