using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Implementations;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffProfilesController : ControllerBase
    {
        private readonly IStaffProfileService _staffService;

        public StaffProfilesController(IStaffProfileService staffService)
        {
            _staffService = staffService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStaffProfileRequest request)
        {
            var success = await _staffService.CreateAsync(request);
            if (!success)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new { success = true, message = "Staff profile created successfully" });
        }

        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetByAccountId(Guid accountId)
        {
            var profile = await _staffService.GetByAccountIdAsync(accountId);
            if (profile == null)
                return NotFound(new { success = false, message = "Staff profile not found" });

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateStaffProfileRequest request)
        {
            var success = await _staffService.UpdateAsync(request);
            if (!success)
                return NotFound(new { success = false, message = "Staff profile not found" });

            return Ok(new { success = true, message = "Staff profile updated successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var profiles = await _staffService.GetAllAsync();
            return Ok(profiles);
        }
    }
}
