using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Services.Implementations;
using MembershipService.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace MembershipService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicPlansController : ControllerBase
    {
        private readonly IBasicPlanService _service;

        public BasicPlansController(IBasicPlanService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBasicPlanRequest request)
        {
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBasicPlanRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
        [HttpPost("batch")]
        public async Task<ActionResult<List<BasicPlanResponse>>> GetBasicPlansByIds([FromBody] List<Guid> ids)
        {
            var result = await _service.GetByIdsAsync(ids);
            return Ok(result);
        }

        [HttpPost("calculate-dynamic-price")]
        public async Task<IActionResult> CalculateDynamicPrice([FromBody] List<Guid> roomIds)
        {
            var totalPrice = await _service.CalculateDynamicPriceFromRoomIdsAsync(roomIds);
            return Ok(new { totalPrice });
        }




        // vũ code
        [HttpGet("{id}/price")]
        public async Task<IActionResult> GetComboPlanPrice(Guid id)
        {
            var plan = await _service.GetByIdAsync(id); 
            if (plan == null)
                return NotFound("Không tìm thấy gói .");

            return Ok(plan.Price); 
        }
        [HttpGet("{id}/duration")]
        public async Task<IActionResult> GetPlanDuration(Guid id)
        {
            var duration = await _service.GetPlanDurationAsync(id);
            if (duration == null)
                return NotFound("Không tìm thấy thời hạn gói.");

            return Ok(duration); // trả trực tiếp DurationDto
        }



    }
}