using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.API.Data;

[ApiController]
[Route("api/locations")]
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

}
