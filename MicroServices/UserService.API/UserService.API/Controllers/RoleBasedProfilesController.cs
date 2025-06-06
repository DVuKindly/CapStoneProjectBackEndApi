using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Implementations;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/userprofiles")]

    public class RoleBasedProfilesController : ControllerBase
    {
        private readonly IStaffProfileService _staffService;
        private readonly ICoachProfileService _coachService;
        private readonly IPartnerProfileService _partnerService;

        public RoleBasedProfilesController(
            IStaffProfileService staffService,
            ICoachProfileService coachService,
            IPartnerProfileService partnerService)
        {
            _staffService = staffService;
            _coachService = coachService;
            _partnerService = partnerService;
        }

        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffProfileRequest request)
        {
            var success = await _staffService.CreateAsync(request);
            if (!success)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new { success = true, message = "Staff profile created successfully" });
        }

        [HttpPost("create-coach")]
        public async Task<IActionResult> CreateCoach([FromBody] CreateCoachProfileRequest request)
        {
            var success = await _coachService.CreateAsync(request);
            if (!success)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new { success = true, message = "Coach profile created successfully" });
        }

        [HttpPost("create-partner")]
        public async Task<IActionResult> CreatePartner([FromBody] CreatePartnerProfileRequest request)
        {
            var success = await _partnerService.CreateAsync(request);
            if (!success)
                return NotFound(new { success = false, message = "User not found" });

            return Ok(new { success = true, message = "Partner profile created successfully" });
        }
    }
}
