namespace BffService.API.DTOs.User.Requests
{
    public class LocationRegionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
