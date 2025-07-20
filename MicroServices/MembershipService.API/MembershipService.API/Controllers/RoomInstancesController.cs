using MembershipService.API.Dtos.Request;
using MembershipService.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomInstancesController : ControllerBase
    {
        private readonly IRoomInstanceService _service;

        public RoomInstancesController(IRoomInstanceService service)
        {
            _service = service;
        }

        [HttpGet("by-option/{optionId}")]
        public async Task<IActionResult> GetByAccommodationOptionId(Guid optionId)
        {
            var result = await _service.GetByAccommodationOptionIdAsync(optionId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms( )
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("by-location/{locationId}")]
        public async Task<IActionResult> GetByLocationId(Guid locationId)
        {
            var result = await _service.GetByLocationIdAsync(locationId);
            return Ok(result);
        }

        //[HttpGet("by-basicPlan/{planId}")]
        //public async Task<IActionResult> GetByBasicPlanId(Guid planId)
        //{
        //    var result = await _service.GetByBasicPlanIdAsync(planId);
        //    return Ok(result);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoomInstanceRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateRoomInstanceRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("{roomInstanceId}/extra-fee")]
        public async Task<IActionResult> GetExtraFee(Guid roomInstanceId)
        {
            var fee = await _service.GetAddOnFeeAsync(roomInstanceId);
            return Ok(new { success = true, data = fee });
        }


    }
}
