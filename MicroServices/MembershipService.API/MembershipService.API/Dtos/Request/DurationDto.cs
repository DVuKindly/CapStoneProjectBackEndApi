namespace MembershipService.API.Dtos.Request
{
    public class DurationDto
    {
        public int Value { get; set; }              // ví dụ: 1
        public string Unit { get; set; } = string.Empty; // ví dụ: "Week"
    }

}
