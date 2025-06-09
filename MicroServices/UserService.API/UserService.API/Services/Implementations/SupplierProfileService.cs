//using UserService.API.Data;
//using UserService.API.DTOs.Requests;
//using UserService.API.Services.Interfaces;

//namespace UserService.API.Services.Implementations
//{
//    public class SupplierProfileService : ISupplierProfileService
//    {
//        private readonly UserDbContext _db;

//        public SupplierProfileService(UserDbContext db)
//        {
//            _db = db;
//        }

//        public async Task<bool> CreateAsync(UserProfilePayload payload)
//        {
//            var supplier = new SupplierProfile
//            {
//                Id = requ
//                AccountId = payload.AccountId,
//                CompanyName = payload.CompanyName,
//                TaxCode = payload.TaxCode,
//                Description = payload.Description,
//                WebsiteUrl = payload.WebsiteUrl,
//                ContactPerson = payload.ContactPerson,
//                ContactPhone = payload.ContactPhone,
//                ContactEmail = payload.ContactEmail,
//                Industry = payload.Industry
//            };

//            await _db.SupplierProfiles.AddAsync(supplier);
//            await _db.SaveChangesAsync();
//            return true;
//        }
//    }
//}
