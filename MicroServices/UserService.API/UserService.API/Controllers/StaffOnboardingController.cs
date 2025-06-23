using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/user/staff-onboarding")]
    [Authorize(Roles = "staff_onboarding")] 
    public class StaffOnboardingController : ControllerBase
    {
        private readonly IStaffOnboardingService _staffService;

        public StaffOnboardingController(IStaffOnboardingService staffService)
        {
            _staffService = staffService;
        }

       
        [HttpPost("approve-membership-request")]
        public async Task<IActionResult> ApproveRequest([FromBody] ApproveMembershipRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var staffId = GetStaffIdFromJwt();
            if (staffId == Guid.Empty)
                return Unauthorized("Không thể xác định Staff từ token.");

            var result = await _staffService.ApproveMembershipRequestAsync(staffId, dto);
            return Ok(result);
        }

       
        private Guid GetStaffIdFromJwt()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userId, out var id) ? id : Guid.Empty;
        }
    }
}
