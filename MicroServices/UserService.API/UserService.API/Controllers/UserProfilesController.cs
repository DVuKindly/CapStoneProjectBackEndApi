using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Interfaces;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/userprofiles")]
    public class UserProfilesController : ControllerBase
    {
        private readonly ILogger<UserProfilesController> _logger;
        private readonly IUserProfileService _userProfileService;
        private readonly ICoachProfileService _coachProfileService;
        private readonly IStaffProfileService _staffProfileService;
        private readonly IPartnerProfileService _partnerProfileService;

        public UserProfilesController(
              ILogger<UserProfilesController> logger,
            IUserProfileService userProfileService,
            ICoachProfileService coachProfileService,
            IStaffProfileService staffProfileService,
            IPartnerProfileService partnerProfileService)
        {
            _logger = logger;
            _userProfileService = userProfileService;
            _coachProfileService = coachProfileService;
            _staffProfileService = staffProfileService;
            _partnerProfileService = partnerProfileService;
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

        [HttpPost("create-coach")]
        public async Task<IActionResult> CreateCoach([FromBody] UserProfilePayload request)
        {
            _logger.LogInformation("🔥 Called /create-coach for {Email}", request.Email);
            return await Create(request);
        }


        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] UserProfilePayload request) => await Create(request);



    }
}
