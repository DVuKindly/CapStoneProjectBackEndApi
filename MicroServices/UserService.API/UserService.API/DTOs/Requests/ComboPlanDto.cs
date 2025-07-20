using System.Text.Json.Serialization;

namespace UserService.API.DTOs.Requests
{
    public class ComboPlanDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        [JsonPropertyName("propertyId")]
        public Guid? LocationId { get; set; }  // ⚠️ Map từ propertyId

        [JsonPropertyName("propertyName")]
        public string LocationName { get; set; } = string.Empty;

        public int PackageDurationValue { get; set; }

        public string PackageDurationUnit { get; set; } = string.Empty;
    }
}
