using MembershipService.API.Dtos.Request;
using MembershipService.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboPlansController : ControllerBase
    {
        private readonly IComboPlanService _service;

        public ComboPlansController(IComboPlanService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ComboPlanCreateRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ComboPlanUpdateRequest request)
        {
            var success = await _service.UpdateAsync(id, request);
            return success ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }


        //vũ code 
        [HttpGet("{id}/price")]
        public async Task<IActionResult> GetComboPlanPrice(Guid id)
        {
            var plan = await _service.GetByIdAsync(id);
            if (plan == null) return NotFound();

            return Ok(plan.TotalPrice);
        }


    }
}
