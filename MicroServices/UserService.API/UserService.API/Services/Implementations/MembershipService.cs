﻿using Microsoft.EntityFrameworkCore;
using SharedKernel.DTOsChung.Request;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;
using UserService.API.Services.Interfaces;

namespace UserService.API.Services.Implementations
{
    public class MembershipService : IMembershipService
    {
        private readonly UserDbContext _db;
        private readonly IAuthServiceClient _authServiceClient;
        private readonly IMembershipServiceClient _membershipServiceClient;

        public MembershipService(
            UserDbContext db,
            IAuthServiceClient authServiceClient,
            IMembershipServiceClient membershipServiceClient)
        {
            _db = db;
            _authServiceClient = authServiceClient;
            _membershipServiceClient = membershipServiceClient;
        }

        public async Task<BaseResponse> CreateMembershipAsync(CreateMembershipDto dto)
        {
            var now = DateTime.UtcNow;

            // ✅ Always check StartDate from PendingMembershipRequests
            DateTime startDate = now;

            if (dto.RequestId.HasValue)
            {
                var request = await _db.PendingMembershipRequests
                    .FirstOrDefaultAsync(r => r.Id == dto.RequestId.Value);

                if (request != null && request.StartDate.HasValue)
                {
                    startDate = request.StartDate.Value;
                }
            }

            var expireAt = AddDuration(startDate, dto.PackageDurationValue, dto.PackageDurationUnit);

            var membership = new Membership
            {
                Id = Guid.NewGuid(),
                AccountId = dto.AccountId,
                PackageId = dto.PackageId,
                PackageType = dto.PackageType,
                PackageName = dto.PackageName,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Paid",
                PaymentTransactionId = dto.PaymentTransactionId,
                PaymentNote = dto.PaymentNote,
                PaymentTime = now,
                PurchasedAt = now,
                StartDate = startDate, // ✅ always from pending request
                ExpireAt = expireAt,
                PackageDurationValue = dto.PackageDurationValue,
                PackageDurationUnit = dto.PackageDurationUnit,
                UsedForRoleUpgrade = dto.UsedForRoleUpgrade,
                PlanSource = dto.PlanSource,
                PendingRequestId = dto.RequestId
            };

            await _db.Memberships.AddAsync(membership);
            await _db.SaveChangesAsync();

            return BaseResponse.Ok("Membership created successfully", membership.Id);
        }




        public async Task<MembershipRequestSummaryDto?> GetMembershipSummaryAsync(Guid membershipId)
        {
            var membership = await _db.Memberships
                .FirstOrDefaultAsync(x => x.Id == membershipId && x.PaymentStatus != "Paid");

            if (membership == null) return null;

            return new MembershipRequestSummaryDto
            {
                MembershipRequestId = membership.Id,
                AccountId = membership.AccountId,
                Amount = membership.Amount,
                RequestedPackageName = membership.PackageName,
                Status = "PendingPayment"
            };
        }

        public async Task<BaseResponse> MarkRequestAsPaidAndApprovedAsync(MarkPaidRequestDto dto)
        {
            if (dto == null || dto.RequestId == Guid.Empty)
                return BaseResponse.Fail("❌ Request không hợp lệ.");

            var membership = await _db.Memberships.FirstOrDefaultAsync(m => m.Id == dto.RequestId);
            if (membership == null)
                return BaseResponse.Fail("❌ Không tìm thấy thông tin Membership.");

            var now = DateTime.UtcNow;

            // ✅ Ưu tiên lấy StartDate từ PendingMembershipRequest
            DateTime startDate = now;
            if (membership.PendingRequestId.HasValue)
            {
                var pendingRequest = await _db.PendingMembershipRequests
                    .FirstOrDefaultAsync(r => r.Id == membership.PendingRequestId.Value);

                if (pendingRequest != null && pendingRequest.StartDate.HasValue)
                {
                    startDate = pendingRequest.StartDate.Value;
                }
            }

            // ✅ Cập nhật thông tin thanh toán
            membership.PaymentTransactionId = dto.PaymentTransactionId;
            membership.PaymentMethod = string.IsNullOrWhiteSpace(dto.PaymentMethod) ? "Unknown" : dto.PaymentMethod;
            membership.PaymentNote = dto.PaymentNote;
            membership.PaymentStatus = "Paid";
            membership.PaymentTime = now;
            membership.PurchasedAt = now;
            membership.StartDate = startDate;

            // ✅ Tính thời hạn nếu là gói basic
            if (membership.PackageType?.ToLower() == "basic" && membership.PackageId != Guid.Empty)
            {
                var duration = await _membershipServiceClient.GetPlanDurationAsync(membership.PackageId, "basic");
                if (duration == null || string.IsNullOrWhiteSpace(duration.Unit))
                    return BaseResponse.Fail("❌ Không lấy được thời hạn gói từ MembershipService.");

                membership.PackageDurationValue = duration.Value;
                membership.PackageDurationUnit = duration.Unit;
                membership.PlanSource = "basic";
                membership.ExpireAt = AddDuration(startDate, duration.Value, duration.Unit);
            }

            // ✅ Nếu dùng để nâng cấp role → gọi AuthService
            if (membership.UsedForRoleUpgrade)
            {
                var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == membership.AccountId);
                if (user != null)
                {
                    user.RoleType = "member";
                    try
                    {
                        var promoted = await _authServiceClient.PromoteUserToMemberAsync(user.AccountId);
                        if (!promoted)
                            return BaseResponse.Fail("❌ Đã thanh toán nhưng nâng cấp vai trò thất bại.");
                    }
                    catch (Exception ex)
                    {
                        return BaseResponse.Fail($"❌ Lỗi khi gọi AuthService: {ex.Message}");
                    }
                }
            }
            
            await _db.SaveChangesAsync();
            return BaseResponse.Ok("✅ Xác nhận thanh toán thành công và cập nhật dữ liệu đầy đủ.");
        }


        public async Task<List<MembershipResponseDto>> GetUserMembershipsAsync(Guid accountId)
        {
            var memberships = await _db.Memberships
                .Where(m => m.AccountId == accountId)
                .OrderByDescending(m => m.PurchasedAt)
                .ToListAsync();

            return memberships.Select(m =>
            {
                var expireAt = m.ExpireAt ?? (m.PackageDurationValue.HasValue && !string.IsNullOrWhiteSpace(m.PackageDurationUnit)
                    ? AddDuration(m.PurchasedAt, m.PackageDurationValue.Value, m.PackageDurationUnit)
                    : m.PurchasedAt);

                return new MembershipResponseDto
                {
                    Id = m.Id,
                    PendingRequestId = m.PendingRequestId,
                    PackageName = m.PackageName,
                    PackageType = m.PackageType,
                    PackageId = m.PackageId,
                    RoomInstanceId = m.RoomInstanceId,

                    Amount = m.Amount,
                    PaymentStatus = m.PaymentStatus ?? "Pending",
                    PaymentMethod = m.PaymentMethod,
                    PurchasedAt = m.PurchasedAt,
                    ExpireAt = expireAt,
                    StartDate = m.StartDate,

                    PackageDurationValue = m.PackageDurationValue,
                    PackageDurationUnit = m.PackageDurationUnit,
             

                    IsActive = m.IsActive,
                    UsedForRoleUpgrade = m.UsedForRoleUpgrade
                };
            }).ToList();
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
                    m.UserProfile.RoleType = "user";
                    await _authServiceClient.DowngradeUserRoleAsync(m.AccountId);
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task<DurationDto?> GetPlanDurationAsync(Guid planId, string planType)
        {
            return await _membershipServiceClient.GetPlanDurationAsync(planId, planType);
        }

        private DateTime AddDuration(DateTime start, int value, string unit)
        {
            return unit.ToLower() switch
            {
                "day" => start.AddDays(value),
                "week" => start.AddDays(7 * value),
                "month" => start.AddMonths(value),
                "year" => start.AddYears(value),
                _ => start
            };
        }
    }
}
