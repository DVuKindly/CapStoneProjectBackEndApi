using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.Entities;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/user/cities")]
    public class CityController : ControllerBase
    {
        private readonly UserDbContext _db;

        public CityController(UserDbContext db)
        {
            _db = db;
        }

        // ✅ GET: /api/user/cities
        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _db.Cities
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description
                })
                .ToListAsync();

            return Ok(cities);
        }

       
        [HttpGet("{id}/exists")]
        public async Task<IActionResult> CityExists(Guid id)
        {
            var exists = await _db.Cities.AnyAsync(c => c.Id == id);
            return exists ? Ok() : NotFound();
        }

        // ✅ POST: /api/user/cities
        [Authorize(Roles = "super_admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDto dto)
        {
            var city = new City
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _db.Cities.Add(city);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                city.Id,
                city.Name,
                city.Description
            });
        }
    }
}
