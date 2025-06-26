using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Enums;
using MembershipService.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static MembershipService.API.Dtos.Request.CreateNextUServiceRequest;

namespace MembershipService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NextUServicesController : ControllerBase
    {
        private readonly INextUServiceService _service;

        public NextUServicesController(INextUServiceService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<NextUServiceResponseDto>> Create([FromBody] CreateNextUServiceRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NextUServiceResponseDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<NextUServiceResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("by-type")]
        public async Task<ActionResult<List<NextUServiceResponseDto>>> GetByServiceType([FromQuery] ServiceType type)
        {
            var result = await _service.GetByServiceTypeAsync(type);
            return Ok(result);
        }

        [HttpGet("by-basic-plan/{basicPlanId}")]
        public async Task<ActionResult<List<NextUServiceResponseDto>>> GetByBasicPlanId(Guid basicPlanId)
        {
            var result = await _service.GetByBasicPlanIdAsync(basicPlanId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NextUServiceResponseDto>> Update(Guid id, [FromBody] UpdateNextUServiceRequest request)
        {
            try
            {
                var result = await _service.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }

    }
}
