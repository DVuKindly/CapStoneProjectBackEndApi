using Microsoft.EntityFrameworkCore;
using SharedKernel.DTOsChung.Request;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Services.Interfaces;


namespace UserService.API.Services.Implementations
{
    public class MembershipService : IMembershipService
    {
        private readonly UserDbContext _db;
        private readonly IAuthServiceClient _authServiceClient;
        private readonly IMembershipServiceClient _membershipServiceClient; // ✅ Thêm inject

        public MembershipService(
            UserDbContext db,
            IAuthServiceClient authServiceClient,
            IMembershipServiceClient membershipServiceClient) // ✅ Inject
        {
            _db = db;
            _authServiceClient = authServiceClient;
            _membershipServiceClient = membershipServiceClient;
        }

        public async Task<BaseResponse> CreateMembershipAsync(CreateMembershipDto dto)
        {
            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                AccountId = dto.AccountId,
                PackageName = dto.PackageName,
                PackageType = dto.PackageType,
              
                Amount = dto.Amount,
                PaymentStatus = "Paid",
                PaymentMethod = dto.PaymentMethod,
                PurchasedAt = DateTime.UtcNow,
                UsedForRoleUpgrade = dto.UsedForRoleUpgrade,
                PlanSource = dto.PlanSource,
                PackageDurationValue = dto.PackageDurationValue,
                PackageDurationUnit = dto.PackageDurationUnit,
                ExpireAt = AddDuration(DateTime.UtcNow, dto.PackageDurationValue, dto.PackageDurationUnit)
            };

            await _db.Memberships.AddAsync(membership);
            await _db.SaveChangesAsync();

            return new BaseResponse
            {
                Success = true,
                Message = "Membership created successfully"
            };
        }

        public async Task<MembershipRequestSummaryDto?> GetMembershipSummaryAsync(Guid membershipId)
        {
            var membership = await _db.Memberships
    .FirstOrDefaultAsync(x => x.Id == membershipId && x.PaymentStatus != "Paid");

            if (membership == null)
                return null;

            return new MembershipRequestSummaryDto
            {
                MembershipRequestId = membership.Id, // Reuse same property
                AccountId = membership.AccountId,
                Amount = membership.Amount,
                RequestedPackageName = membership.PackageName,
                Status = "PendingPayment"
            };
        }


        public async Task<bool> MarkMembershipAsPaidAsync(MarkPaidRequestDto dto)
        {
            var membership = await _db.Memberships.FirstOrDefaultAsync(x => x.Id == dto.RequestId);
            if (membership == null) return false;

            membership.PaymentMethod = dto.PaymentMethod;
            membership.PaymentTransactionId = dto.PaymentTransactionId;
            membership.PaymentNote = dto.PaymentNote;
            membership.PaymentStatus = "Paid";
            membership.PaymentTime = DateTime.UtcNow;

            // ✅ Chưa tính hạn sử dụng → để null hoặc xử lý sau
            // Có thể ghi log để theo dõi sau này cần update thêm
            // membership.ExpireAt = null;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<MembershipResponseDto>> GetUserMembershipsAsync(Guid accountId)
        {
            var memberships = await _db.Memberships
                .Where(m => m.AccountId == accountId)
                .OrderByDescending(m => m.PurchasedAt)
                .ToListAsync();

            return memberships.Select(m =>
            {
                var purchasedAt = m.PurchasedAt;
                var expireAt = m.ExpireAt;

                if (expireAt == null && m.PackageDurationValue != null && !string.IsNullOrEmpty(m.PackageDurationUnit))
                {
                    expireAt = AddDuration(purchasedAt, m.PackageDurationValue.Value, m.PackageDurationUnit!);
                }

                return new MembershipResponseDto
                {
                    Id = m.Id,
                    PackageName = m.PackageName,
                    PackageType = m.PackageType,
                    Amount = m.Amount,
                    PaymentStatus = m.PaymentStatus ?? "Pending",
                    PaymentMethod = m.PaymentMethod,
                    PurchasedAt = m.PurchasedAt,
                    ExpireAt = expireAt ?? m.PurchasedAt,
                    PackageDurationValue = m.PackageDurationValue,
                    PackageDurationUnit = m.PackageDurationUnit,
                    IsActive = m.IsActive,
                    UsedForRoleUpgrade = m.UsedForRoleUpgrade,
                    PlanSource = m.PlanSource
                };
            }).ToList();
        }

        private DateTime AddDuration(DateTime start, int value, string unit)
        {
            return unit.ToLower() switch
            {
                "day" => start.AddDays(value),
                "month" => start.AddMonths(value),
                "year" => start.AddYears(value),
                _ => start
            };
        }

        public async Task CheckAndDowngradeExpiredMembershipsAsync()
        {
            var now = DateTime.UtcNow;

            var expired = await _db.Memberships
                .Where(m => m.UsedForRoleUpgrade && m.ExpireAt.HasValue && m.ExpireAt < now)
                .Include(m => m.UserProfile)
                .ToListAsync();

            foreach (var m in expired)
            {
                if (m.UserProfile != null)
                {
                    m.UserProfile.RoleType = "user"; // downgrade
                    await _authServiceClient.DowngradeUserRoleAsync(m.AccountId);
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
