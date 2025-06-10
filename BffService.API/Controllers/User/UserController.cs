using BffService.API.DTOs.Requests;
using BffService.API.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BffService.API.Controllers.User
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserServiceClient _userServiceClient;

        public UserController(IUserServiceClient userServiceClient)
        {
            _userServiceClient = userServiceClient;
        }

        [HttpGet("profileme")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var accountId = GetAccountIdFromToken();
            var result = await _userServiceClient.GetUserProfileAsync(accountId);
            return result != null ? Ok(result) : NotFound("Không tìm thấy hồ sơ.");
        }

        [HttpPut("updateprofile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
        {
            var accountId = GetAccountIdFromToken();
            var response = await _userServiceClient.UpdateUserProfileAsync(accountId, dto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("request-membership")]
        [Authorize]
        public async Task<IActionResult> SubmitMembership([FromBody] MembershipRequestDto dto)
        {
            var accountId = GetAccountIdFromToken();
            var result = await _userServiceClient.SubmitMembershipRequestAsync(accountId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("membership/history")]
        [Authorize]
        public async Task<IActionResult> GetHistory()
        {
            var accountId = GetAccountIdFromToken();
            var data = await _userServiceClient.GetRequestHistoryAsync(accountId);
            return Ok(data);
        }

        [HttpGet("membership/pending")]
        [Authorize(Roles = "staff_onboarding")]
        public async Task<IActionResult> GetPending()
        {
            var accountId = GetAccountIdFromToken();
            var data = await _userServiceClient.GetPendingRequestsForStaffAsync(accountId);
            return Ok(data);
        }

        [HttpPost("membership/approve")]
        [Authorize(Roles = "staff_onboarding")]
        public async Task<IActionResult> Approve([FromBody] ApproveMembershipRequestDto dto)
        {
            var result = await _userServiceClient.ApproveMembershipRequestAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("membership/reject")]
        [Authorize(Roles = "staff_onboarding")]
        public async Task<IActionResult> Reject([FromBody] RejectMembershipRequestDto dto)
        {
            var result = await _userServiceClient.RejectMembershipRequestAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("getLocation")]
        public async Task<IActionResult> GetLocation()
        {
            var data = await _userServiceClient.GetAllLocationsAsync();
            return data != null ? Ok(data) : NotFound("Không tìm thấy yêu cầu.");
        }
        [HttpGet("membership/detail/{id}")]
        [Authorize(Roles = "staff_onboarding")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var data = await _userServiceClient.GetRequestDetailAsync(id);
            return data != null ? Ok(data) : NotFound("Không tìm thấy yêu cầu.");
        }

        private Guid GetAccountIdFromToken()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }
    }
}
