using MembershipService.API.Dtos.Request;
using MembershipService.API.Services.Implementations;
using MembershipService.API.Services.Interfaces; // ✅ Sửa import interface
using Microsoft.AspNetCore.Mvc;

namespace MembershipService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntitlementRuleController : ControllerBase
    {
        private readonly IEntitlementRuleService _service; // ✅ Đổi sang interface

        public EntitlementRuleController(IEntitlementRuleService service) // ✅ Inject interface
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEntitlementRuleDto dto) => Ok(await _service.CreateAsync(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEntitlementRuleDto dto)
        {
            if (id != dto.Id) return BadRequest("Id không khớp");
            return Ok(await _service.UpdateAsync(dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => Ok(await _service.DeleteAsync(id));

        [HttpGet("by-basicplan/{basicPlanId}")]
        public async Task<IActionResult> GetByBasicPlanId(Guid basicPlanId)
        {
            var result = await _service.GetByBasicPlanIdAsync(basicPlanId);
            return Ok(result);
        }

        [HttpGet("total-price/by-basicplan/{basicPlanId}")]
        public async Task<IActionResult> GetTotalEntitlementPrice(Guid basicPlanId)
        {
            var totalPrice = await _service.GetTotalEntitlementPriceAsync(basicPlanId);
            return Ok(totalPrice);
        }

    }
}
