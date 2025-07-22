using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;

[ApiController]
[Route("api/user/locations")]

public class PropertyController : ControllerBase
{
    private readonly UserDbContext _db;

    public PropertyController(UserDbContext db)
    {
        _db = db;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var locations = await _db.Propertys
            .Include(p => p.Location)
            .ThenInclude(l => l.City)
            .ToListAsync();

        var result = locations.Select(p => new
        {
            p.Id,
            p.Name,
            p.Description,
            LocationId = p.Location != null ? p.Location.Id : Guid.Empty,
            LocationName = p.Location?.Name ?? "UnKnow",
            CityId = p.Location?.City != null ? p.Location.City.Id : Guid.Empty,
            CityName = p.Location?.City?.Name ?? "UnKnow"
        }).ToList();

        return Ok(result);
    }

    [HttpGet("{id}/display-name")]
    public async Task<IActionResult> GetDisplayName(Guid id)
    {
        var property = await _db.Propertys
            .Include(p => p.Location)
            .ThenInclude(l => l.City)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (property == null) return NotFound();

        var displayName = $"{property.Name}, {property.Location?.Name}, {property.Location?.City?.Name}";
        return Ok(displayName);
    }


    [HttpGet("{id}/exists")]
    public async Task<IActionResult> Exists(Guid id)
    {
        var exists = await _db.Propertys.AnyAsync(l => l.Id == id);
        return exists ? Ok() : NotFound();
    }
    [Authorize(Roles = "super_admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLocationDto dto)
    {
        var location = new Property
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        _db.Propertys.Add(location);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            location.Id,
            location.Name,
            location.Description
        });
    }

    [HttpGet("{propertyId}/in-city/{cityId}")]
    public async Task<IActionResult> IsPropertyInCity(Guid propertyId, Guid cityId)
    {
        var exists = await _db.Propertys
            .Include(p => p.Location)
            .AnyAsync(p => p.Id == propertyId && p.Location != null && p.Location.CityId == cityId);

        return exists ? Ok() : NotFound();
    }
    [HttpGet("/api/user/locations/by-city/{cityId}")]
    public async Task<IActionResult> GetLocationsByCity(Guid cityId)
    {
        var locations = await _db.Locations
            .Where(l => l.CityId == cityId)
            .ToListAsync();

        var result = locations.Select(l => new
        {
            l.Id,
            l.Name,
            l.Description,
            l.CityId
        });

        return Ok(result);
    }
    [HttpGet("propertyby-location/{locationId}")]
    public async Task<IActionResult> GetPropertiesByLocation(Guid locationId)
    {
        var properties = await _db.Propertys
            .Where(p => p.LocationId == locationId)
            .Include(p => p.Location)
            .ThenInclude(l => l.City)
            .ToListAsync();

        var result = properties.Select(p => new
        {
            p.Id,
            p.Name,
            p.Description,
            LocationId = p.LocationId,
            LocationName = p.Location?.Name,
            CityId = p.Location?.CityId,
            CityName = p.Location?.City?.Name
        });

        return Ok(result);
    }


}
