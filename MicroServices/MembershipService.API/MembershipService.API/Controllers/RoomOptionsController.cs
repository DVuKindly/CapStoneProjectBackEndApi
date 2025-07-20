using MembershipService.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MembershipService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomOptionsController : ControllerBase
    {
        private readonly IRoomSizeOptionService _roomSizeService;
        private readonly IRoomViewOptionService _roomViewService;
        private readonly IRoomFloorOptionService _roomFloorService;
        private readonly IBedTypeOptionService _bedTypeService;

        public RoomOptionsController(
            IRoomSizeOptionService roomSizeService,
            IRoomViewOptionService roomViewService,
            IRoomFloorOptionService roomFloorService,
            IBedTypeOptionService bedTypeService)
        {
            _roomSizeService = roomSizeService;
            _roomViewService = roomViewService;
            _roomFloorService = roomFloorService;
            _bedTypeService = bedTypeService;
        }

        [HttpGet("sizes")]
        public async Task<IActionResult> GetRoomSizes()
        {
            var result = await _roomSizeService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("views")]
        public async Task<IActionResult> GetRoomViews()
        {
            var result = await _roomViewService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("floors")]
        public async Task<IActionResult> GetRoomFloors()
        {
            var result = await _roomFloorService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("bed-types")]
        public async Task<IActionResult> GetBedTypes()
        {
            var result = await _bedTypeService.GetAllAsync();
            return Ok(result);
        }

    }
}
