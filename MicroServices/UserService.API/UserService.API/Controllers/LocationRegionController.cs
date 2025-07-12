using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;

[ApiController]
[Route("api/user/locations")]

public class LocationRegionController : ControllerBase
{
    private readonly UserDbContext _db;

    public LocationRegionController(UserDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var locations = await _db.LocationRegions
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
        var exists = await _db.LocationRegions.AnyAsync(l => l.Id == id);
        return exists ? Ok() : NotFound();
    }
    [Authorize(Roles = "super_admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLocationDto dto)
    {
        var location = new LocationRegion
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        _db.LocationRegions.Add(location);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            location.Id,
            location.Name,
            location.Description
        });
    }



}
