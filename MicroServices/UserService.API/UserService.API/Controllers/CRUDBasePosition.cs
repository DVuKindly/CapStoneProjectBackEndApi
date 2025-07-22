using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs.BasePositon;
using UserService.API.Services.Implementations;
using UserService.API.Services.Interfaces;

namespace UserService.API.Controllers
{
    [Route("api/basePosition")]
    [ApiController]
    public class CRUDBasePositionController : ControllerBase
    {
        private readonly IBasePositionService _service;

        public CRUDBasePositionController(IBasePositionService service)
        {
            _service = service;
        }

        // === CITIES ===
        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
            => Ok(await _service.GetCitiesAsync());

        [HttpPost("cities")]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityBasePositionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _service.CreateCityAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ ERROR in CreateCity: " + ex.Message);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("cities/{id}")]
        public async Task<IActionResult> UpdateCity(Guid id, [FromBody] UpdateCityDto dto)
            => Ok(await _service.UpdateCityAsync(id, dto));

        [HttpDelete("cities/{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
            => Ok(await _service.DeleteCityAsync(id));


        // === LOCATIONS ===
        [HttpGet("locations")]
        public async Task<IActionResult> GetLocations()
            => Ok(await _service.GetLocationsAsync());

        [HttpPost("locations")]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationBasePositionDTO dto)
            => Ok(await _service.CreateLocationAsync(dto));

        [HttpPut("locations/{id}")]
        public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] UpdateLocationDto dto)
            => Ok(await _service.UpdateLocationAsync(id, dto));

        [HttpDelete("locations/{id}")]
        public async Task<IActionResult> DeleteLocation(Guid id)
            => Ok(await _service.DeleteLocationAsync(id));


        // === PROPERTIES ===
        [HttpGet("properties")]
        public async Task<IActionResult> GetProperties()
            => Ok(await _service.GetPropertiesAsync());

        [HttpPost("properties")]
        public async Task<IActionResult> CreateProperty([FromBody] CreatePropertyBasePositionDTO dto)
            => Ok(await _service.CreatePropertyAsync(dto));

        [HttpPut("properties/{id}")]
        public async Task<IActionResult> UpdateProperty(Guid id, [FromBody] UpdatePropertyDto dto)
            => Ok(await _service.UpdatePropertyAsync(id, dto));

        [HttpDelete("properties/{id}")]
        public async Task<IActionResult> DeleteProperty(Guid id)
            => Ok(await _service.DeletePropertyAsync(id));
    }
}
