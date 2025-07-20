using System.Text.Json.Serialization;
using UserService.API.DTOs.Responses;

public class BasicPlanResponse : IPlanResponse
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool VerifyBuy { get; set; }

    public int PackageDurationId { get; set; }

    public string PackageDurationName { get; set; } = string.Empty;

    [JsonPropertyName("propertyId")]
    public Guid LocationId { get; set; }  // ⚠️ Map từ propertyId

    [JsonPropertyName("propertyName")]
    public string LocationName { get; set; } = string.Empty;

    public List<Guid> NextUServiceIds { get; set; } = new();
}
