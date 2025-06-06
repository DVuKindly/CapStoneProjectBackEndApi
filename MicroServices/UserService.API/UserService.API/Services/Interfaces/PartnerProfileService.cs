using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.Entities;
using UserService.API.Services.Implementations;

namespace UserService.API.Services.Interfaces
{
    public class PartnerProfileService : IPartnerProfileService
    {
        private readonly UserDbContext _db;

        public PartnerProfileService(UserDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(CreatePartnerProfileRequest request)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if (user == null) return false;

            var partner = new PartnerProfile
            {
                Id = request.AccountId,
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
            return true;
        }
    }
}
