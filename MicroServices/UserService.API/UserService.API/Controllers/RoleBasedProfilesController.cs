using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.Entities;

namespace UserService.API.Controllers
{
    [ApiController]
    [Route("api/userprofiles")]
    public class RoleBasedProfilesController : ControllerBase
    {
        private readonly UserDbContext _db;

        public RoleBasedProfilesController(UserDbContext db)
        {
            _db = db;
        }

        [HttpPost("create-staff")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffProfileRequest request)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if (user == null)
                return NotFound(new { success = false, message = "User not found" });

            var staff = new StaffProfile
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                Phone = request.Phone,
                Department = request.Department,
                Note = request.Note,
                Level = request.Level,
                Address = request.Address,
                ManagerId = request.ManagerId,
                JoinedDate = DateTime.UtcNow,
                IsActive = true
            };

            _db.StaffProfiles.Add(staff);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, message = "Staff profile created successfully" });
        }

        [HttpPost("create-coach")]
        public async Task<IActionResult> CreateCoach([FromBody] CreateCoachProfileRequest request)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if (user == null)
                return NotFound(new { success = false, message = "User not found" });

            var coach = new CoachProfile
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                CoachType = request.CoachType,
                Specialty = request.Specialty,
                ModuleInCharge = request.ModuleInCharge,
                Region = request.Region,
                ExperienceYears = request.ExperienceYears,
                Bio = request.Bio,
                Certifications = request.Certifications,
                LinkedInUrl = request.LinkedInUrl
            };

            _db.CoachProfiles.Add(coach);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, message = "Coach profile created successfully" });
        }

        [HttpPost("create-partner")]
        public async Task<IActionResult> CreatePartner([FromBody] CreatePartnerProfileRequest request)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if (user == null)
                return NotFound(new { success = false, message = "User not found" });

            var partner = new PartnerProfile
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                OrganizationName = request.OrganizationName,
                PartnerType = request.PartnerType,
                Location = request.Location,
                ContractUrl = request.ContractUrl,
                RepresentativeName = request.RepresentativeName,
                RepresentativePhone = request.RepresentativePhone,
                RepresentativeEmail = request.RepresentativeEmail,
                Description = request.Description,
                WebsiteUrl = request.WebsiteUrl,
                Industry = request.Industry,
                CreatedByAdminId = request.CreatedByAdminId,
                JoinedAt = DateTime.UtcNow,
                IsActivated = true,
                ActivatedAt = DateTime.UtcNow
            };

            _db.PartnerProfiles.Add(partner);
            await _db.SaveChangesAsync();

            return Ok(new { success = true, message = "Partner profile created successfully" });
        }
    }
}
