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
            .Select(x => new {
                x.Id,
                x.Name,
                x.Description
            })
            .ToListAsync();

        return Ok(locations);
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


}
