using MembershipService.API.Dtos.Response;
using MembershipService.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicPlanTypesController : ControllerBase
    {
        private readonly IBasicPlanTypeService _service;

        public BasicPlanTypesController(IBasicPlanTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<BasicPlanTypeResponseDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BasicPlanTypeResponseDto>> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
