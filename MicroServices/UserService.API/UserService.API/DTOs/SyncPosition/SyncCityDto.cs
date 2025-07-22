namespace UserService.API.DTOs.SyncPosition
{
    public class SyncCityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
