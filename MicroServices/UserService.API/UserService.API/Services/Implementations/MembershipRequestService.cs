using Microsoft.EntityFrameworkCore;
using SharedKernel.DTOsChung.Request;
using System.Numerics;
using UserService.API.Constants;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;
using UserService.API.Services.Implementations;
using UserService.API.Services.Interfaces;

public class MembershipRequestService : IMembershipRequestService
{
    private readonly UserDbContext _db;
    private readonly IAuthServiceClient _authServiceClient;
    private readonly IMembershipServiceClient _membershipServiceClient;
    private readonly IPaymentServiceClient _paymentServiceClient;

    public MembershipRequestService(UserDbContext db , IAuthServiceClient authServiceClient, IMembershipServiceClient membershipServiceClient, IPaymentServiceClient paymentServiceClient)
    {
        _membershipServiceClient = membershipServiceClient;
        _db = db;
        _authServiceClient = authServiceClient;
        _paymentServiceClient = paymentServiceClient;
    }

    public async Task<BaseResponse> ApproveMembershipRequestAsync(Guid staffAccountId, ApproveMembershipRequestDto dto)
    {
        // 1. Get staff's region (LocationId)
        var staffRegionId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffRegionId == Guid.Empty)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Staff location not found."
            };
        }

        // 2. Find the request that belongs to this staff's region and is pending
        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r =>
                r.Id == dto.RequestId &&
                r.LocationId == staffRegionId &&
                r.Status == "Pending");

        if (request == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Request not found or access denied."
            };
        }

        // 3. Update request status to pending payment
        request.Status = "PendingPayment";
        request.ApprovedAt = DateTime.UtcNow;
        request.StaffNote = dto.StaffNote;

        // 4. Update onboarding status
        var user = request.UserProfile!;
        user.OnboardingStatus = "PendingPayment";

        await _db.SaveChangesAsync();

        return new BaseResponse
        {
            Success = true,
            Message = "Membership request approved successfully."
        };
    }




    public async Task<BaseResponse> RejectMembershipRequestAsync(Guid staffAccountId, RejectMembershipRequestDto dto)
    {
        // 1. Get staff's region (LocationId)
        var staffRegionId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffRegionId == Guid.Empty)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Staff location not found."
            };
        }

        // 2. Find valid pending request in this region
        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r =>
                r.Id == dto.RequestId &&
                r.Status == "Pending" &&
                r.LocationId == staffRegionId);

        if (request == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Request not found or access denied."
            };
        }

        // 3. Reject the request
        request.Status = "Rejected";
        request.StaffNote = dto.Reason;
        request.ApprovedAt = DateTime.UtcNow;

        // 4. Update user's onboarding status
        if (request.UserProfile != null)
        {
            request.UserProfile.OnboardingStatus = "Rejected";
        }

        await _db.SaveChangesAsync();

        return new BaseResponse
        {
            Success = true,
            Message = "Membership request has been rejected successfully."
        };
    }





    public async Task<List<PendingMembershipRequestDto>> GetPendingRequestsForStaffAsync(Guid staffAccountId)
    {
        // 1. Lấy LocationId của staff
        var staffLocationId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffLocationId == Guid.Empty)
            return new List<PendingMembershipRequestDto>();

        // 2. Lấy danh sách request có cùng LocationId và status = Pending
        var pendingRequests = await _db.PendingMembershipRequests
            .Where(r => r.LocationId == staffLocationId && r.Status == "Pending")
            .Include(r => r.UserProfile)
            .ToListAsync();

        if (!pendingRequests.Any())
            return new List<PendingMembershipRequestDto>();

        // 3. Lấy tất cả PackageId duy nhất để gọi MembershipService
        var packageIds = pendingRequests
            .Where(r => r.PackageId.HasValue)
            .Select(r => r.PackageId!.Value)
            .Distinct()
            .ToList();

        var plans = await _membershipServiceClient.GetBasicPlansByIdsAsync(packageIds);
        var planDict = plans.ToDictionary(p => p.Id, p => p);

        // 4. Mapping sang DTO đầy đủ
        var result = pendingRequests.Select(r =>
        {
            var user = r.UserProfile;
            planDict.TryGetValue(r.PackageId ?? Guid.Empty, out var plan);

            return new PendingMembershipRequestDto
            {
                RequestId = r.Id,
                FullName = user?.FullName,
                RequestedPackageName = r.RequestedPackageName,
                PackageType = r.PackageType,
                Amount = r.Amount,
                ExpireAt = r.ExpireAt,
                Status = r.Status,
                PaymentStatus = r.PaymentStatus,
                PaymentMethod = r.PaymentMethod,
                PaymentTime = r.PaymentTime,
                StaffNote = r.StaffNote,
                ApprovedAt = r.ApprovedAt,

                MessageToStaff = r.MessageToStaff,
                CreatedAt = r.CreatedAt,
                LocationName = plan?.LocationName ?? "Không rõ",

                Interests = r.Interests,
                PersonalityTraits = r.PersonalityTraits,
                Introduction = r.Introduction,
                CvUrl = r.CvUrl,

                ExtendedProfile = new ExtendedProfileDto
                {
                    Gender = user?.Gender,
                    DOB = user?.DOB,
                    AvatarUrl = user?.AvatarUrl,
                    SocialLinks = user?.SocialLinks,
                    Address = user?.Address,
                    RoleType = user?.RoleType
                }
            };
        }).ToList();

        return result;
    }







    public async Task<BaseResponse> SubmitRequestAsync(Guid accountId, MembershipRequestDto dto)
    {
        if (dto.PackageId == Guid.Empty)
            return BaseResponse.Fail("Thiếu mã gói dịch vụ.");

        var packageType = dto.PackageType?.ToLower();
        if (packageType != "basic" && packageType != "combo")
            return BaseResponse.Fail("Loại gói không hợp lệ. Chỉ được chọn 'basic' hoặc 'combo'.");

        var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (user == null)
            return BaseResponse.Fail("Không tìm thấy hồ sơ người dùng.");

        if (!IsUserProfileCompleted(user))
            return BaseResponse.Fail("Vui lòng hoàn tất hồ sơ cá nhân trước khi gửi yêu cầu.");

        var existingRequest = await _db.PendingMembershipRequests
            .Where(r => r.AccountId == accountId && r.PackageId == dto.PackageId &&
                        (r.Status == "Pending" || r.Status == "PendingPayment" || r.Status == "Completed"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();

        if (existingRequest != null)
        {
            if (existingRequest.Status == "Completed" || existingRequest.Status == "Pending")
                return BaseResponse.Fail("Bạn đã gửi hoặc đã mua gói này rồi. Không thể gửi lại yêu cầu.");

            if (existingRequest.Status == "PendingPayment")
            {
                var elapsed = DateTime.UtcNow - existingRequest.CreatedAt;
                if (elapsed.TotalMinutes < 15)
                    return BaseResponse.Fail("Bạn đã tạo yêu cầu mua gói này và chưa thanh toán. Vui lòng hoàn tất thanh toán hoặc đợi 15 phút để gửi lại.");

                _db.PendingMembershipRequests.Remove(existingRequest);
                await _db.SaveChangesAsync();
            }
        }

        Guid locationId;
        string name;
        decimal price;

        if (packageType == "basic")
        {
            var plan = await _membershipServiceClient.GetBasicPlanByIdAsync(dto.PackageId);
            var duration = await _membershipServiceClient.GetPlanDurationAsync(dto.PackageId, "basic");
            if (plan == null || duration == null || string.IsNullOrWhiteSpace(duration.Unit))
                return BaseResponse.Fail("Không thể lấy thông tin gói hoặc thời hạn từ MembershipService.");

            var priceInfo = await _membershipServiceClient.GetPlanPriceAsync(dto.PackageId, "basic");
            price = Convert.ToDecimal(priceInfo);
            locationId = plan.LocationId ?? Guid.Empty;
            name = plan.Name;

            var request = BuildRequest(accountId, dto.PackageId, name, price, locationId, user, dto.MessageToStaff, "basic");
            request.PackageDurationValue = duration.Value;
            request.PackageDurationUnit = duration.Unit;

            if (plan.VerifyBuy)
            {
                request.Status = "PendingPayment";
                _db.PendingMembershipRequests.Add(request);
                await _db.SaveChangesAsync();

                if (string.IsNullOrWhiteSpace(dto.RedirectUrl))
                    return BaseResponse.Fail("Thiếu RedirectUrl để chuyển hướng sau thanh toán.");

                var paymentDto = new CreatePaymentRequestDto
                {
                    RequestId = request.Id,
                    AccountId = accountId,
                    PackageId = request.PackageId,
                    Amount = request.Amount,
                    PackageType = "basic",
                    PaymentMethod = "VNPAY",
                    RedirectUrl = dto.RedirectUrl
                };

                var paymentResponse = await _paymentServiceClient.CreatePaymentRequestAsync(paymentDto);
                if (!paymentResponse.Success)
                    return BaseResponse.Fail("Tạo thanh toán thất bại: " + paymentResponse.Message);

                return BaseResponse.Ok("Yêu cầu đã được tạo và chuyển sang thanh toán.", new
                {
                    IsDirectPurchase = true,
                    RequestId = request.Id,
                    PaymentUrl = paymentResponse.Data
                });
            }
            else
            {
                request.Status = "Pending";
                _db.PendingMembershipRequests.Add(request);
                await _db.SaveChangesAsync();

                return BaseResponse.Ok("Yêu cầu của bạn đã được gửi. Vui lòng chờ xét duyệt.", new
                {
                    IsDirectPurchase = false,
                    RequestId = request.Id
                });
            }
        }

        // ✅ Combo Package (Sửa đầy đủ)
        if (packageType == "combo")
        {
            var combo = await _membershipServiceClient.GetComboPlanByIdAsync(dto.PackageId);
            var duration = await _membershipServiceClient.GetPlanDurationAsync(dto.PackageId, "combo");
            if (combo == null || duration == null || string.IsNullOrWhiteSpace(duration.Unit))
                return BaseResponse.Fail("Không thể lấy thông tin gói Combo hoặc thời hạn từ MembershipService.");

            var priceInfo = await _membershipServiceClient.GetPlanPriceAsync(dto.PackageId, "combo");
            price = priceInfo;
            locationId = combo.LocationId ?? Guid.Empty;
            name = combo.Name;

            var request = BuildRequest(accountId, dto.PackageId, name, price, locationId, user, dto.MessageToStaff, "combo");
            request.PackageDurationValue = duration.Value;
            request.PackageDurationUnit = duration.Unit;

            request.Status = "Pending";
            _db.PendingMembershipRequests.Add(request);
            await _db.SaveChangesAsync();

            return BaseResponse.Ok("Yêu cầu mua gói Combo đã được gửi. Vui lòng chờ xét duyệt.", new
            {
                IsDirectPurchase = false,
                RequestId = request.Id
            });
        }

        return BaseResponse.Fail("Xảy ra lỗi không xác định.");
    }


    private PendingMembershipRequest BuildRequest(
     Guid accountId,
     Guid packageId,
     string name,
     decimal price,
     Guid locationId,
     UserProfile user,
     string? messageToStaff,
     string packageType)
    {
        return new PendingMembershipRequest
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            PackageId = packageId,
            RequestedPackageName = name ?? "Unknown",         // ⚠️ Nếu name null
            Amount = price,
            LocationId = locationId,
            Interests = user.Interests ?? "",
            PersonalityTraits = user.PersonalityTraits ?? "",
            Introduction = user.Introduction ?? "",
            CvUrl = user.CvUrl ?? "",
            MessageToStaff = messageToStaff ?? "",
            CreatedAt = DateTime.UtcNow,
            PackageType = packageType ?? "basic"
        };
    }


















    private bool IsUserProfileCompleted(UserProfile user)
    {
        return !string.IsNullOrWhiteSpace(user.FullName)
            && !string.IsNullOrWhiteSpace(user.Phone)
            && !string.IsNullOrWhiteSpace(user.Gender)
            && user.DOB.HasValue
            && !string.IsNullOrWhiteSpace(user.AvatarUrl)
            && !string.IsNullOrWhiteSpace(user.Interests)
              && !string.IsNullOrWhiteSpace(user.Address)
            && !string.IsNullOrWhiteSpace(user.PersonalityTraits)
            && !string.IsNullOrWhiteSpace(user.Introduction)
            && !string.IsNullOrWhiteSpace(user.CvUrl);
    }

    public async Task<PendingMembershipRequestDto?> GetRequestDetailAsync(Guid requestId, Guid staffAccountId)
    {
        // Lấy khu vực của staff
        var staffRegionId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffRegionId == Guid.Empty)
            return null;

        // Lấy yêu cầu kèm user
        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r =>
                r.Id == requestId &&
                r.LocationId == staffRegionId);

        if (request == null)
            return null;

        var user = request.UserProfile;

        // Lấy thông tin membership nếu đã thanh toán
        var membership = await _db.Memberships
            .Where(m => m.AccountId == request.AccountId)
            .OrderByDescending(m => m.PurchasedAt)
            .FirstOrDefaultAsync();

        // Lấy location name từ bảng LocationRegions
        var locationName = await _db.LocationRegions
            .Where(l => l.Id == request.LocationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync();

        return new PendingMembershipRequestDto
        {
            RequestId = request.Id,
            FullName = user?.FullName,
            RequestedPackageName = request.RequestedPackageName,
            PackageType = request.PackageType,
            Amount = membership?.Amount ?? request.Amount,
            ExpireAt = membership?.ExpireAt ?? request.ExpireAt,
            Status = request.Status,
            PaymentStatus = request.PaymentStatus,
            PaymentMethod = membership?.PaymentMethod ?? request.PaymentMethod,
            PaymentTime = membership?.PaymentTime ?? request.PaymentTime,
            StaffNote = request.StaffNote,
            ApprovedAt = request.ApprovedAt,
            MessageToStaff = request.MessageToStaff,
            CreatedAt = request.CreatedAt,
            LocationName = locationName,
            Interests = request.Interests,
            PersonalityTraits = request.PersonalityTraits,
            Introduction = request.Introduction,
            CvUrl = request.CvUrl,
            ExtendedProfile = new ExtendedProfileDto
            {
                Gender = user?.Gender,
                DOB = user?.DOB,
                AvatarUrl = user?.AvatarUrl,
                SocialLinks = user?.SocialLinks,
                Address = user?.Address,
                RoleType = user?.RoleType
            }
        };
    }








    public async Task<List<PendingMembershipRequestDto>> GetRequestHistoryAsync(Guid accountId)
    {
        var result = new List<PendingMembershipRequestDto>();

        // 1. Lấy user info
        var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);

        // 2. Lấy tất cả PendingMembershipRequests
        var pendingRequests = await _db.PendingMembershipRequests
            .Where(r => r.AccountId == accountId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        // 3. Lấy tất cả Memberships đã mua
        var memberships = await _db.Memberships
            .Where(m => m.AccountId == accountId)
            .OrderByDescending(m => m.PurchasedAt)
            .ToListAsync();

        // 4. Tập hợp tất cả PackageId để gọi MembershipService 1 lần
        var packageIds = pendingRequests
            .Where(r => r.PackageId.HasValue)
            .Select(r => r.PackageId!.Value)
            .Concat(memberships.Select(m => m.PackageId))
            .Distinct()
            .ToList();

        var basicPlans = await _membershipServiceClient.GetBasicPlansByIdsAsync(packageIds);
        var basicPlanDict = basicPlans.ToDictionary(p => p.Id, p => p);

        // 5. Mapping từ PendingMembershipRequest
        foreach (var r in pendingRequests)
        {
            basicPlanDict.TryGetValue(r.PackageId ?? Guid.Empty, out var plan);

            result.Add(new PendingMembershipRequestDto
            {
                RequestId = r.Id,
                FullName = user?.FullName,
                RequestedPackageName = r.RequestedPackageName,
                MessageToStaff = r.MessageToStaff,
                CreatedAt = r.CreatedAt,
                LocationName = plan?.LocationName,
                Interests = r.Interests,
                PersonalityTraits = r.PersonalityTraits,
                Introduction = r.Introduction,
                CvUrl = r.CvUrl,
                PackageType = r.PackageType,
                Status = r.Status,
                Amount = (decimal)r.Amount!,
                PaymentStatus = r.PaymentStatus,
                PaymentMethod = r.PaymentMethod,
                PaymentTime = r.PaymentTime,
                ExtendedProfile = new ExtendedProfileDto
                {
                    Gender = user?.Gender,
                    DOB = user?.DOB,
                    AvatarUrl = user?.AvatarUrl,
                    SocialLinks = user?.SocialLinks,
                    Address = user?.Address,
                    RoleType = user?.RoleType
                }
            });
        }

        // 6. Mapping từ Membership
        foreach (var m in memberships)
        {
            basicPlanDict.TryGetValue(m.PackageId, out var plan);

            result.Add(new PendingMembershipRequestDto
            {
                RequestId = m.Id,
                FullName = user?.FullName,
                RequestedPackageName = m.PackageName,
                MessageToStaff = null,
                CreatedAt = m.PurchasedAt,
                LocationName = plan?.LocationName,
                Interests = user?.Interests,
                PersonalityTraits = user?.PersonalityTraits,
                Introduction = user?.Introduction,
                CvUrl = user?.CvUrl,
                PackageType = m.PackageType,
                Status = "Completed",
                Amount = m.Amount,
                PaymentStatus = m.PaymentStatus,
                PaymentMethod = m.PaymentMethod,
                PaymentTime = m.PaymentTime,
                ExpireAt = m.ExpireAt, // ✅ THÊM DÒNG NÀY
                ExtendedProfile = new ExtendedProfileDto
                {
                    Gender = user?.Gender,
                    DOB = user?.DOB,
                    AvatarUrl = user?.AvatarUrl,
                    SocialLinks = user?.SocialLinks,
                    Address = user?.Address,
                    RoleType = user?.RoleType
                }
            });
        }

        // 7. Sắp xếp chung theo thời gian
        return result.OrderByDescending(r => r.CreatedAt).ToList();
    }


    public async Task<MembershipRequestSummaryDto?> GetMembershipRequestSummaryAsync(Guid requestId)
    {
        // 1️⃣ Ưu tiên tìm trong bảng PendingMembershipRequests
        var request = await _db.PendingMembershipRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request != null)
        {
            if (request.Status != "PendingPayment")
                return null;

            // ✅ Validate loại gói
            var planType = request.PackageType?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(planType) || (planType != "basic" && planType != "combo"))
                throw new ArgumentException($"Loại gói không hợp lệ: {request.PackageType}");

            // ✅ Gọi MembershipService để lấy giá gói từ hệ thống gốc
            decimal amount = await _membershipServiceClient.GetPlanPriceAsync(request.PackageId!.Value, planType);

            return new MembershipRequestSummaryDto
            {
                MembershipRequestId = request.Id,
                AccountId = request.AccountId,
                PackageId = request.PackageId,
                RequestedPackageName = request.RequestedPackageName,
                Amount = amount,
                Status = request.Status
            };
        }

        // 2️⃣ Nếu không có request, thử tra trong bảng Memberships (trường hợp thanh toán trực tiếp)
        var membership = await _db.Memberships
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == requestId);

        if (membership != null)
        {
            return new MembershipRequestSummaryDto
            {
                MembershipRequestId = membership.Id,
                AccountId = membership.AccountId,
                PackageId = membership.PackageId,
                RequestedPackageName = membership.PackageName,
                Amount = membership.Amount,
                Status = "PendingPayment" // gán mặc định để Payment xử lý
            };
        }

        // 3️⃣ Không tìm thấy hợp lệ
        return null;
    }





    public async Task<List<PendingMembershipRequestDto>> GetAllRequestsForStaffLocationAsync(Guid staffAccountId)
    {
        // 1. Lấy khu vực mà Staff đang phụ trách
        var staffLocationId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffLocationId == Guid.Empty)
            return new();

        // 2. Lấy danh sách request trong khu vực đó
        var requests = await _db.PendingMembershipRequests
            .Where(r => r.LocationId == staffLocationId)
            .Include(r => r.UserProfile)
            .ToListAsync();

        if (!requests.Any())
            return new();

        // 3. Lấy accountId và packageId để xử lý Membership và giá
        var accountIds = requests.Select(r => r.AccountId).Distinct().ToList();

        // 4. Lấy Membership đã thanh toán của user (nếu có)
        var memberships = await _db.Memberships
            .Where(m => accountIds.Contains(m.AccountId))
            .ToListAsync();

        var membershipDict = memberships
            .GroupBy(m => m.AccountId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(m => m.PurchasedAt).First());

        // 5. Lấy tên khu vực (LocationRegion.Name) theo LocationId
        var locationRegions = await _db.LocationRegions
            .Where(l => l.Id == staffLocationId)
            .ToDictionaryAsync(l => l.Id, l => l.Name);

        // 6. Chuẩn bị danh sách kết quả
        var result = requests.Select(r =>
        {
            var user = r.UserProfile;
            membershipDict.TryGetValue(r.AccountId, out var membership);
            locationRegions.TryGetValue(r.LocationId ?? Guid.Empty, out var regionName);

            return new PendingMembershipRequestDto
            {
                RequestId = r.Id,
                FullName = user?.FullName,
                RequestedPackageName = r.RequestedPackageName,
                PackageType = r.PackageType,
                Amount = membership?.Amount ?? r.Amount,
                ExpireAt = membership?.ExpireAt ?? r.ExpireAt,
                Status = r.Status,
                PaymentStatus = r.PaymentStatus,
                PaymentMethod = membership?.PaymentMethod ?? r.PaymentMethod,
                PaymentTime = membership?.PaymentTime ?? r.PaymentTime,
                StaffNote = r.StaffNote,
                ApprovedAt = r.ApprovedAt,
                MessageToStaff = r.MessageToStaff,
                CreatedAt = r.CreatedAt,
                LocationName = regionName ?? string.Empty, // ✅ Lấy từ LocationRegions
                Interests = r.Interests,
                PersonalityTraits = r.PersonalityTraits,
                Introduction = r.Introduction,
                CvUrl = r.CvUrl,
                ExtendedProfile = new ExtendedProfileDto
                {
                    Gender = user?.Gender,
                    DOB = user?.DOB,
                    AvatarUrl = user?.AvatarUrl,
                    SocialLinks = user?.SocialLinks,
                    Address = user?.Address,
                    RoleType = user?.RoleType
                }
            };
        }).ToList();

        return result;
    }









    public async Task<BaseResponse> MarkRequestAsPaidAndApprovedAsync(MarkPaidRequestDto dto)
    {
        if (dto == null || dto.RequestId == Guid.Empty)
            return BaseResponse.Fail("Dữ liệu không hợp lệ.");

        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r => r.Id == dto.RequestId);

        if (request == null)
            return BaseResponse.Fail("Không tìm thấy yêu cầu đăng ký.");

        if (request.Status == "Completed")
            return BaseResponse.Fail("Yêu cầu đã được xử lý trước đó.");

        var paidTime = dto.PaidTime ?? DateTime.UtcNow;

        // ✅ Cập nhật trạng thái thanh toán
        request.PaymentStatus = "Paid";
        request.PaymentMethod = dto.PaymentMethod ?? "Unknown";
        request.PaymentTransactionId = dto.PaymentTransactionId;
        request.PaymentTime = paidTime;
        request.PaymentNote = dto.PaymentNote;
        request.Status = "Completed";

        // ✅ Cập nhật trạng thái onboarding & nâng role nếu chưa là member
        if (request.UserProfile != null)
        {
            request.UserProfile.OnboardingStatus = "ApprovedMember";

            if (request.UserProfile.RoleType != "member")
            {
                request.UserProfile.RoleType = "member";

                try
                {
                    var promoted = await _authServiceClient.PromoteUserToMemberAsync(request.AccountId);
                    if (!promoted)
                    {
                        Console.WriteLine("⚠️ Đã xác nhận thanh toán nhưng không thể nâng vai trò.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"🚨 Lỗi nâng vai trò: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("✅ User đã là member, không cần nâng role.");
            }
        }

        // ✅ Lấy dữ liệu thời hạn từ PendingMembershipRequests
        int? durationValue = request.PackageDurationValue;
        string? durationUnit = request.PackageDurationUnit;
        DateTime? expireAt = null;

        if (durationValue.HasValue && !string.IsNullOrWhiteSpace(durationUnit))
        {
            expireAt = CalculateExpireDate(paidTime, durationValue.Value, durationUnit);
        }

        // ✅ Tạo bản ghi Membership
        var membership = new Membership
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            PackageId = request.PackageId ?? Guid.Empty,
            PackageName = request.RequestedPackageName ?? "Gói không tên",
            PackageType = request.PackageType?.ToLower() ?? "basic",
            Amount = request.Amount ?? 0,
            LocationId = request.LocationId ?? Guid.Empty,
            PurchasedAt = paidTime,
            UsedForRoleUpgrade = request.PackageType?.ToLower() == "combo",

            // Thanh toán
            PaymentMethod = request.PaymentMethod,
            PaymentStatus = request.PaymentStatus,
            PaymentNote = request.PaymentNote,
            PaymentTime = request.PaymentTime,
            PaymentTransactionId = request.PaymentTransactionId,

            // Thời hạn (TỪ BẢNG `PendingMembershipRequests`)
            PackageDurationValue = durationValue,
            PackageDurationUnit = durationUnit,
            ExpireAt = expireAt
        };

        _db.Memberships.Add(membership);
        await _db.SaveChangesAsync();

        return BaseResponse.Ok("Đã xác nhận thanh toán và cập nhật Membership thành công.", new
        {
            MembershipId = membership.Id,
            PackageName = membership.PackageName,
            PackageType = membership.PackageType,
            Upgraded = membership.UsedForRoleUpgrade,
            ExpireAt = membership.ExpireAt
        });
    }




    private DateTime CalculateExpireDate(DateTime start, int value, string unit)
    {
        return unit.ToLower() switch
        {
            "day" => start.AddDays(value),
            "month" => start.AddMonths(value),
            "year" => start.AddYears(value),
            _ => start
        };
    }

}
