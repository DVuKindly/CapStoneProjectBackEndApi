using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class SupplierProfileService : ISupplierProfileService
    {
        private readonly UserDbContext _db;

        public SupplierProfileService(UserDbContext db)
        {
            _db = db;
        }

        public async Task<BaseResponse> CreateAsync(UserProfilePayload request)
        {
            var exists = await _db.UserProfiles.AnyAsync(x => x.AccountId == request.AccountId);
            if (!exists)
            {
                return new BaseResponse { Success = false, Message = "User profile chưa tồn tại để liên kết supplier." };
            }

            var supplier = new SupplierProfile
            {
                AccountId = request.AccountId,
                CompanyName = request.OrganizationName,
                ContactPerson = request.RepresentativeName,
                ContactPhone = request.RepresentativePhone,
                ContactEmail = request.RepresentativeEmail,
                Description = request.Description,
                WebsiteUrl = request.WebsiteUrl,
                Industry = request.Industry,
                TaxCode = request.Note,
                LocationId = request.LocationId
            };

            await _db.SupplierProfiles.AddAsync(supplier);
            await _db.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Supplier profile created successfully."
            };
        }
    }
}
