using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class PartnerProfileService : IPartnerProfileService
    {
        private readonly UserDbContext _db;

        public PartnerProfileService(UserDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(UserProfilePayload payload)
        {
            var partner = new PartnerProfile
            {
                Id = Guid.NewGuid(),
                AccountId = payload.AccountId,
                OrganizationName = payload.OrganizationName,
                PartnerType = payload.PartnerType,
                RepresentativeName = payload.RepresentativeName,
                RepresentativePhone = payload.RepresentativePhone,
                RepresentativeEmail = payload.RepresentativeEmail,
                Description = payload.Description,
                WebsiteUrl = payload.WebsiteUrl,
                Industry = payload.Industry
            };

            await _db.PartnerProfiles.AddAsync(partner);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
