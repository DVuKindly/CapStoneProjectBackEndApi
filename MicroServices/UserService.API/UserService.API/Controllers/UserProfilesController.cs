using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs.Requests;

using UserService.API.Services.Implementations;
using UserService.API.Services.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/userprofiles")]
    public class UserProfilesController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfilesController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserProfileRequest request)
        {
            var success = await _userProfileService.CreateAsync(request);
            if (!success)
                return BadRequest(new { success = false, message = "Tạo hồ sơ thất bại" });

            return Ok(new { success = true, message = "Tạo hồ sơ thành công" });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserProfileRequest request)
        {
            var success = await _userProfileService.UpdateAsync(request);
            if (!success)
                return NotFound(new { success = false, message = "Không tìm thấy user để cập nhật" });

            return Ok(new { success = true, message = "Cập nhật thành công" });
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetByAccountId(Guid accountId)
        {
            var profile = await _userProfileService.GetByAccountIdAsync(accountId);
            if (profile == null)
                return NotFound(new { success = false, message = "Không tìm thấy user" });

            return Ok(profile);
        }

        [HttpGet("status/{accountId}")]
        public async Task<IActionResult> GetStatus(Guid accountId)
        {
            var status = await _userProfileService.GetStatusAsync(accountId);
            if (status == null)
                return NotFound(new { success = false, message = "Không tìm thấy user" });

            return Ok(status);
        }

        [HttpGet("check-can-promote/{accountId}")]
        public async Task<IActionResult> CheckCanPromote(Guid accountId)
        {
            var result = await _userProfileService.CheckCanPromoteAsync(accountId);
            return Ok(result);
        }

        [HttpGet("incomplete")]
        public async Task<IActionResult> GetIncomplete()
        {
            var list = await _userProfileService.GetIncompleteProfilesAsync();
            return Ok(list);
        }
    }
}
