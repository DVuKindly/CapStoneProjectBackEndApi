using Microsoft.AspNetCore.Mvc;
using UserService.API.Constants;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Requests.UserService.API.DTOs.Requests;
using UserService.API.Entities;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfilesController : ControllerBase
    {
        private readonly UserDbContext _context;

        public UserProfilesController(UserDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserProfileRequest request)
        {
            var role = string.IsNullOrEmpty(request.RoleType) ? "User" : request.RoleType;

            DateTime? dob = null;
            if (!string.IsNullOrEmpty(request.DOB) && DateTime.TryParse(request.DOB, out var parsedDob))
            {
                dob = parsedDob;
            }

            var profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId.ToString(),
                FullName = request.FullName,
                RoleType = role,
                Phone = request.Phone,
                Gender = request.Gender,
                DOB = dob,
                Location = request.Location,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsCompleted = false,
                IsVerifiedByAdmin = false,
                OnboardingStatus = OnboardingStatuses.PendingPackageSelection
            };

            await _context.UserProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }


}

