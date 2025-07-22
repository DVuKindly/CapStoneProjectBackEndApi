using System.ComponentModel.DataAnnotations;

namespace UserService.API.DTOs.BasePositon
{
    public class CreateLocationBasePositionDTO
    {
        [Required]
        public Guid CityId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }
    }

    public class UpdateLocationDto : CreateLocationBasePositionDTO { }

    public class LocationResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public Guid CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
    }

}
