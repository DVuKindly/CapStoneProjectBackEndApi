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
        public async Task<IActionResult> Create([FromBody] CreateComboPlanRequest request)
    => Ok(await _service.CreateAsync(request));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateComboPlanRequest request)
            => Ok(await _service.UpdateAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => Ok(await _service.DeleteAsync(id));
    }
}
