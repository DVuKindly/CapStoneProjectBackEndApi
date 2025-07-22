namespace UserService.API.DTOs.SyncPosition
{
    public class SyncPropertyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? LocationId { get; set; }
    }
}
