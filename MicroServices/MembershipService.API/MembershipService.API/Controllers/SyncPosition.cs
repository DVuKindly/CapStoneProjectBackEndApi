using MembershipService.API.Data;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Controllers
{
    [Route("api/membership/syncPosition")]
    [ApiController]
    public class SyncPositionController : ControllerBase
    {
        private readonly MembershipDbContext _db;

        public SyncPositionController(MembershipDbContext db)
        {
            _db = db;
        }

        [HttpPost("cities")]
        public async Task<IActionResult> SyncCity([FromBody] SyncCityDto dto)
        {
            var city = await _db.Cities.FindAsync(dto.Id);
            if (city == null)
            {
                city = new City
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description
                };
                _db.Cities.Add(city);
            }
            else
            {
                city.Name = dto.Name;
                city.Description = dto.Description;
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("locations")]
        public async Task<IActionResult> SyncLocation([FromBody] SyncLocationDto dto)
        {
            var location = await _db.Locations.FindAsync(dto.Id);
            if (location == null)
            {
                location = new Location
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    //Description = dto.Description,
                    CityId = dto.CityId
                };
                _db.Locations.Add(location);
            }
            else
            {
                location.Name = dto.Name;
                //location.Description = dto.Description;
                location.CityId = dto.CityId;
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("properties")]
        public async Task<IActionResult> SyncProperty([FromBody] SyncPropertyDto dto)
        {
            var property = await _db.Propertys.FindAsync(dto.Id);
            if (property == null)
            {
                property = new Property
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    LocationId = dto.LocationId
                };
                _db.Propertys.Add(property);
            }
            else
            {
                property.Name = dto.Name;
                property.Description = dto.Description;
                property.LocationId = dto.LocationId;
            }

            await _db.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("cities/{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            var city = await _db.Cities.FindAsync(id);
            if (city == null)
                return NotFound("City not found.");

            var locations = await _db.Locations.Where(l => l.CityId == id).ToListAsync();
            if (locations.Any())
                return BadRequest("Cannot delete city because it has related locations.");

            _db.Cities.Remove(city);
            await _db.SaveChangesAsync();
            return Ok("City deleted.");
        }

        [HttpDelete("locations/{id}")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            var location = await _db.Locations.FindAsync(id);
            if (location == null)
                return NotFound("Location not found.");

            var properties = await _db.Propertys.Where(p => p.LocationId == id).ToListAsync();
            if (properties.Any())
                return BadRequest("Cannot delete location because it has related properties.");

            _db.Locations.Remove(location);
            await _db.SaveChangesAsync();
            return Ok("Location deleted.");
        }


        [HttpDelete("properties/{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
        {
            var property = await _db.Propertys.FindAsync(id);
            if (property == null)
                return NotFound("Property not found.");

            _db.Propertys.Remove(property);
            await _db.SaveChangesAsync();
            return Ok("Property deleted.");
        }

    }

}
