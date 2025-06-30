namespace MembershipService.API.Entities
{
    public class BasicPlanRoom
    {
        public Guid Id { get; set; }
        public Guid BasicPlanId { get; set; }
        public BasicPlan BasicPlan { get; set; }

        public Guid RoomId { get; set; }
        public RoomInstance RoomInstance { get; set; }

        public int NightsIncluded { get; set; } // Số đêm được sử dụng
        public decimal? CustomPricePerNight { get; set; } // Nếu có
        public decimal? TotalPrice { get; set; } // = Nights * Price
    }
}
