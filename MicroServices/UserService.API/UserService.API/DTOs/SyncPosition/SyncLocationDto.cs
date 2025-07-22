namespace UserService.API.DTOs.SyncPosition
{
    public class SyncLocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CityId { get; set; }
    }
}
