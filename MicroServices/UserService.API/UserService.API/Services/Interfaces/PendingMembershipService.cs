using Microsoft.EntityFrameworkCore;
using UserService.API.Constants;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class PendingMembershipService : IPendingMembershipService
    {
        private readonly UserDbContext _db;

        public PendingMembershipService(UserDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateRequestAsync(CreatePendingMembershipRequest request)
        {
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if (user == null) return false;

            var exists = await _db.PendingMembershipRequests.AnyAsync(p => p.AccountId == request.AccountId);
            if (exists) return false;

            var pending = new PendingMembershipRequest
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                PackageId = request.PackageId,
                RequestedPackageName = request.RequestedPackageName, // ✅ Thêm dòng này
                Location = request.Location,
                CreatedAt = DateTime.UtcNow,
                Status = OnboardingStatuses.PendingApproval
            };


            await _db.PendingMembershipRequests.AddAsync(pending);
            user.OnboardingStatus = OnboardingStatuses.PendingApproval;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<PendingMembershipRequestResponse>> GetAllPendingAsync()
        {
            return await _db.PendingMembershipRequests
                .Include(p => p.UserProfile)
                .Where(p => p.Status == OnboardingStatuses.PendingApproval)
                .Select(p => new PendingMembershipRequestResponse
                {
                    AccountId = p.AccountId,
                    FullName = p.UserProfile.FullName,
                    Location = p.Location,
                    PackageId = p.PackageId,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<List<PendingMembershipRequestResponse>> GetPendingByLocationAsync(string location)
        {
            return await _db.PendingMembershipRequests
                .Include(p => p.UserProfile)
                .Where(p => p.Status == OnboardingStatuses.PendingApproval && p.Location == location)
                .Select(p => new PendingMembershipRequestResponse
                {
                    AccountId = p.AccountId,
                    FullName = p.UserProfile.FullName,
                    Location = p.Location,
                    PackageId = p.PackageId,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> ApproveAsync(ApprovePendingMembershipRequest request)
        {
            var pending = await _db.PendingMembershipRequests.FirstOrDefaultAsync(p => p.AccountId == request.AccountId);
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if (pending == null || user == null) return false;

            pending.Status = OnboardingStatuses.ApprovedAwaitingPayment;
            user.OnboardingStatus = OnboardingStatuses.ApprovedAwaitingPayment;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectAsync(RejectPendingMembershipRequest request)
        {
            var pending = await _db.PendingMembershipRequests.FirstOrDefaultAsync(p => p.AccountId == request.AccountId);
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if (pending == null || user == null) return false;

            pending.Status = "Rejected";
            user.OnboardingStatus = "Rejected";
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmPaymentAsync(Guid accountId)
        {
            var pending = await _db.PendingMembershipRequests.FirstOrDefaultAsync(p => p.AccountId == accountId);
            var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);
            if (pending == null || user == null) return false;

            pending.Status = OnboardingStatuses.PaymentConfirmed;
            user.OnboardingStatus = OnboardingStatuses.Activated;
            user.IsVerifiedByAdmin = true;
            user.IsCompleted = true;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<PendingMembershipRequestResponse?> GetByAccountIdAsync(Guid accountId)
        {
            var pending = await _db.PendingMembershipRequests
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(p => p.AccountId == accountId);

            if (pending == null) return null;

            return new PendingMembershipRequestResponse
            {
                AccountId = pending.AccountId,
                FullName = pending.UserProfile.FullName,
                Location = pending.Location,
                PackageId = pending.PackageId,
                Status = pending.Status,
                CreatedAt = pending.CreatedAt
            };
        }
    }
}
