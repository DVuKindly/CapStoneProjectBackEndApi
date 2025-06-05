using Microsoft.AspNetCore.Mvc;
using UserService.API.Constants;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
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
            var profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId.ToString(),
                FullName = request.FullName,
                RoleType = "User",
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
