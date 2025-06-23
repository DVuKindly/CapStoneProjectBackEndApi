using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/user/profiles")]
    public class UserProfilesController : ControllerBase
    {
        private readonly ILogger<UserProfilesController> _logger;
        private readonly IUserProfileService _userProfileService;
        private readonly ICoachProfileService _coachProfileService;
        private readonly IStaffProfileService _staffProfileService;
        private readonly IPartnerProfileService _partnerProfileService;
        private readonly ISupplierProfileService _supplierProfileService;

        public UserProfilesController(
              ILogger<UserProfilesController> logger,
            IUserProfileService userProfileService,
            ICoachProfileService coachProfileService,
            IStaffProfileService staffProfileService,
            IPartnerProfileService partnerProfileService,
            ISupplierProfileService supplierProfileService)
        {
            _logger = logger;
            _userProfileService = userProfileService;
            _coachProfileService = coachProfileService;
            _staffProfileService = staffProfileService;
            _partnerProfileService = partnerProfileService;
            _supplierProfileService = supplierProfileService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserProfilePayload request)
        {
            var response = await _userProfileService.CreateAsync(request);
            if (!response.Success)
            {
                return BadRequest(new { success = false, message = response.Message });
            }

            var role = request.RoleType?.ToLower();

            switch (role)
            {
                case "coaching":
                    await _coachProfileService.CreateAsync(request);
                    break;
                case "partner":
                    await _partnerProfileService.CreateAsync(request);
                    break;
                case "staff_onboarding":
                case "staff_service":
                case "staff_content":
                    await _staffProfileService.CreateAsync(request);
                    break;
                case "supplier":
                    await _supplierProfileService.CreateAsync(request);
                    break;
                case "manager":
                case "user":
                case "admin":
                    // không cần tạo thêm profile riêng → chỉ cần UserProfile là đủ
                    break;
                default:
                    _logger.LogWarning("⛔ Unknown role: {Role}", request.RoleType);
                    break;
            }

            return Ok(new { success = true, message = "Tạo hồ sơ thành công." });
        }
        [HttpPost("create-partner")]
        public async Task<IActionResult> CreatePartner([FromBody] UserProfilePayload request) => await Create(request);
        [HttpPost("create-supplier")]
        public async Task<IActionResult> CreateSuplỉe([FromBody] UserProfilePayload request) => await Create(request);

        [HttpPost("create-coach")]
        public async Task<IActionResult> CreateCoach([FromBody] UserProfilePayload request)
        {
            _logger.LogInformation("🔥 Called /create-coach for {Email}", request.Email);
            return await Create(request);
        }


        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] UserProfilePayload request) => await Create(request);
        // 📌 GET: api/user-profile/me
        [HttpGet("profileme")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var accountId = GetAccountIdFromToken();
            if (accountId == Guid.Empty) return Unauthorized();

            var profile = await _userProfileService.GetProfileAsync(accountId);
            return profile == null ? NotFound("Không tìm thấy hồ sơ.") : Ok(profile);
        }

        [HttpPut("updateprofile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var accountId = GetAccountIdFromToken();
            if (accountId == Guid.Empty) return Unauthorized();

            var response = await _userProfileService.UpdateProfileAsync(accountId, dto);
            return response.Success ? Ok(response) : NotFound(response);
        }


        private Guid GetAccountIdFromToken()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
        }


    }
}
