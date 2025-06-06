using UserService.API.DTOs.Requests;
using UserService.API.Entities;
using UserService.API.Repositories.Implementations;
using UserService.API.Services.Implementations;

namespace UserService.API.Services.Interfaces
{
    public class CoachProfileService : ICoachProfileService
    {
        private readonly ICoachProfileRepository _repo;

        public CoachProfileService(ICoachProfileRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreateAsync(CreateCoachProfileRequest request)
        {
            var user = await _repo.GetUserByAccountIdAsync(request.AccountId);
            if (user == null) return false;

            var coach = new CoachProfile
            {
                Id = request.AccountId,
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

            await _repo.AddCoachProfileAsync(coach);
            return true;
        }
    }

}
