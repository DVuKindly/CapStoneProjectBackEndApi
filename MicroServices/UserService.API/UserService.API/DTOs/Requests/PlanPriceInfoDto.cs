namespace UserService.API.DTOs.Requests
{
    public class PlanPriceInfoDto
    {
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public string? DurationUnit { get; set; } // day, month, year
        public int DurationValue { get; set; }
        public string? PlanSource { get; set; }
        public string? Currency { get; set; }
    }

}
