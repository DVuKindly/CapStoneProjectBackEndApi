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

    public MembershipRequestService(UserDbContext db, IAuthServiceClient authServiceClient, IMembershipServiceClient membershipServiceClient, IPaymentServiceClient paymentServiceClient)
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
        // 1. Get staff's LocationId
        var staffLocationId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffLocationId == Guid.Empty)
            return new List<PendingMembershipRequestDto>();

        // 2. Get pending requests with same location
        var pendingRequests = await _db.PendingMembershipRequests
            .Where(r => r.LocationId == staffLocationId && r.Status == "Pending")
            .Include(r => r.UserProfile)
                .ThenInclude(u => u.UserPersonalityTraits)
                    .ThenInclude(t => t.PersonalityTrait)
            .Include(r => r.UserProfile)
                .ThenInclude(u => u.UserSkills)
                    .ThenInclude(s => s.Skill)
            .ToListAsync();

        if (!pendingRequests.Any())
            return new List<PendingMembershipRequestDto>();

        // 3. Get plan info from MembershipService
        var packageIds = pendingRequests
            .Where(r => r.PackageId.HasValue)
            .Select(r => r.PackageId!.Value)
            .Distinct()
            .ToList();

        var plans = await _membershipServiceClient.GetBasicPlansByIdsAsync(packageIds);
        var planDict = plans.ToDictionary(p => p.Id, p => p);

        // 4. Map to DTOs
        var result = pendingRequests.Select(r =>
        {
            var user = r.UserProfile;
            planDict.TryGetValue(r.PackageId ?? Guid.Empty, out var plan);

            var traits = user?.UserPersonalityTraits?.Select(t => t.PersonalityTrait.Name) ?? new List<string>();
            var skills = user?.UserSkills?.Select(s => s.Skill.Name) ?? new List<string>();
            var combinedTraits = traits.Concat(skills).ToList();

            return new PendingMembershipRequestDto
            {
                RequestId = r.Id,
                FullName = user?.FullName,
                RequestedPackageName = r.RequestedPackageName,
                PackageType = r.PackageType,
                Amount = r.Amount,
                AddOnsFee = r.AddOnsFee, // ✅ THÊM DÒNG NÀY
                ExpireAt = r.ExpireAt,
                Status = r.Status,
                PaymentStatus = r.PaymentStatus,
                PaymentMethod = r.PaymentMethod,
                PaymentTime = r.PaymentTime,
                StaffNote = r.StaffNote,
                ApprovedAt = r.ApprovedAt,

                MessageToStaff = r.MessageToStaff,
                CreatedAt = r.CreatedAt,
                LocationName = plan?.LocationName ?? "Unknown",

                Interests = r.Interests,
                PersonalityTraits = string.Join(", ", combinedTraits),
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




























    private bool IsUserProfileCompleted(UserProfile user)
    {
        return !string.IsNullOrWhiteSpace(user.FullName)
            && !string.IsNullOrWhiteSpace(user.Phone)
            && !string.IsNullOrWhiteSpace(user.Gender)
            && user.DOB.HasValue
            && !string.IsNullOrWhiteSpace(user.AvatarUrl)
            && user.UserInterests != null && user.UserInterests.Any()
            && ((user.UserPersonalityTraits != null && user.UserPersonalityTraits.Any())
                || (user.UserSkills != null && user.UserSkills.Any()))
            && !string.IsNullOrWhiteSpace(user.Introduction)
            && !string.IsNullOrWhiteSpace(user.CvUrl);
    }


    public async Task<PendingMembershipRequestDto?> GetRequestDetailAsync(Guid requestId, Guid staffAccountId)
    {
        // 1. Lấy khu vực của staff
        var staffRegionId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffRegionId == Guid.Empty)
            return null;

        // 2. Lấy request kèm user profile, interests, traits, skills
        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
                .ThenInclude(u => u.UserInterests)
                    .ThenInclude(i => i.Interest)
            .Include(r => r.UserProfile)
                .ThenInclude(u => u.UserPersonalityTraits)
                    .ThenInclude(pt => pt.PersonalityTrait)
            .Include(r => r.UserProfile)
                .ThenInclude(u => u.UserSkills)
                    .ThenInclude(s => s.Skill)
            .FirstOrDefaultAsync(r => r.Id == requestId && r.LocationId == staffRegionId);

        if (request == null)
            return null;

        var user = request.UserProfile;

        // 3. Lấy thông tin Membership nếu đã mua gói (ưu tiên MembershipData mới hơn RequestData)
        var membership = await _db.Memberships
            .Where(m => m.AccountId == request.AccountId && m.PackageId == request.PackageId)
            .OrderByDescending(m => m.PurchasedAt)
            .FirstOrDefaultAsync();

        // 4. Lấy tên location
        var locationName = await _db.Propertys
            .Where(l => l.Id == request.LocationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync();

        // 5. Ghép tên sở thích, traits, skills
        var interestNames = user?.UserInterests.Select(i => i.Interest.Name).ToList() ?? new();
        var personalityNames = user?.UserPersonalityTraits.Select(t => t.PersonalityTrait.Name).ToList() ?? new();
        var skillNames = user?.UserSkills.Select(s => s.Skill.Name).ToList() ?? new();

        // 6. Trả về DTO hoàn chỉnh
        return new PendingMembershipRequestDto
        {
            RequestId = request.Id,
            FullName = user?.FullName,
            RequestedPackageName = request.RequestedPackageName,
            PackageType = request.PackageType,
            Amount = membership?.Amount ?? request.Amount,
            AddOnsFee = request.AddOnsFee,
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

            Interests = string.Join(", ", interestNames),
            PersonalityTraits = string.Join(", ", personalityNames.Concat(skillNames)),

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

        var user = await _db.UserProfiles
            .Include(u => u.UserInterests).ThenInclude(i => i.Interest)
            .Include(u => u.UserPersonalityTraits).ThenInclude(p => p.PersonalityTrait)
            .Include(u => u.UserSkills).ThenInclude(s => s.Skill)
            .FirstOrDefaultAsync(u => u.AccountId == accountId);

        var memberships = await _db.Memberships
            .Where(m => m.AccountId == accountId)
            .OrderByDescending(m => m.PurchasedAt)
            .ToListAsync();

        var paidPackageIds = memberships.Select(m => m.PackageId).ToHashSet();

        var pendingRequests = await _db.PendingMembershipRequests
            .Where(r => r.AccountId == accountId
                && (!r.PackageId.HasValue || !paidPackageIds.Contains(r.PackageId.Value)))
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        // Tách Basic và Combo PlanId
        var basicIds = new List<Guid>();
        var comboIds = new List<Guid>();

        var allRequests = memberships.Select(m => (m.PackageId, m.PackageType))
            .Concat(pendingRequests
                .Where(r => r.PackageId.HasValue)
                .Select(r => (r.PackageId!.Value, r.PackageType)));

        foreach (var (id, type) in allRequests.Distinct())
        {
            if (type?.ToLower() == "basic")
                basicIds.Add(id);
            else if (type?.ToLower() == "combo")
                comboIds.Add(id);
        }

        // Gọi cả hai API
        var basicPlans = await _membershipServiceClient.GetBasicPlansByIdsAsync(basicIds);
        var comboPlans = await _membershipServiceClient.GetComboPlansByIdsAsync(comboIds);

        var planDict = basicPlans.Cast<IPlanResponse>()
            .Concat(comboPlans)
            .ToDictionary(p => p.Id, p => p);

        // 1️⃣ Map từ PendingMembershipRequests
        foreach (var r in pendingRequests)
        {
            planDict.TryGetValue(r.PackageId ?? Guid.Empty, out var plan);

            result.Add(new PendingMembershipRequestDto
            {
                RequestId = r.Id,
                PackageId = r.PackageId,
                FullName = user?.FullName,
                RequestedPackageName = r.RequestedPackageName,
                PackageType = r.PackageType,
                Amount = r.Amount ?? 0,
                StartDate = r.StartDate,
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
                RoomInstanceId = r.RoomInstanceId,
                RequireBooking = r.RequireBooking,
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

        // 2️⃣ Map từ Memberships
        foreach (var m in memberships)
        {
            planDict.TryGetValue(m.PackageId, out var plan);

            var interestNames = user?.UserInterests?.Select(i => i.Interest.Name).ToList() ?? new();
            var traitNames = user?.UserPersonalityTraits?.Select(p => p.PersonalityTrait.Name).ToList() ?? new();
            var skillNames = user?.UserSkills?.Select(s => s.Skill.Name).ToList() ?? new();

            result.Add(new PendingMembershipRequestDto
            {
                RequestId = m.Id,
                PackageId = m.PackageId,
                FullName = user?.FullName,
                RequestedPackageName = m.PackageName,
                RoomInstanceId = m.RoomInstanceId,
                RequireBooking = m.RoomInstanceId != null,
                PackageType = m.PackageType,
                Amount = m.Amount,
                StartDate = m.StartDate,
                ExpireAt = m.ExpireAt,
                Status = "Completed",
                PaymentStatus = m.PaymentStatus,
                PaymentMethod = m.PaymentMethod,
                PaymentTime = m.PaymentTime,
                CreatedAt = m.PurchasedAt,
                MessageToStaff = null,
                StaffNote = null,
                ApprovedAt = null,
                LocationName = plan?.LocationName ?? "Không rõ",
                Interests = string.Join(", ", interestNames),
                PersonalityTraits = string.Join(", ", traitNames.Concat(skillNames)),
                Introduction = user?.Introduction,
                CvUrl = user?.CvUrl,
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
                Amount = request.Amount ?? 0, 
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
        // 1. Lấy khu vực của staff
        var staffLocationId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffLocationId == Guid.Empty)
            return new();

        // 2. Lấy toàn bộ request trong khu vực
        var requests = await _db.PendingMembershipRequests
            .Where(r => r.LocationId == staffLocationId)
            .Include(r => r.UserProfile)
                .ThenInclude(u => u.UserInterests)
                    .ThenInclude(i => i.Interest)
            .Include(r => r.UserProfile)
                .ThenInclude(u => u.UserPersonalityTraits)
                    .ThenInclude(t => t.PersonalityTrait)
            .Include(r => r.UserProfile)
                .ThenInclude(u => u.UserSkills)
                    .ThenInclude(s => s.Skill)
            .ToListAsync();

        if (!requests.Any())
            return new();

        // 3. Lấy các accountId duy nhất
        var accountIds = requests.Select(r => r.AccountId).Distinct().ToList();

        // 4. Lấy Membership gần nhất cho từng account (nếu có)
        var memberships = await _db.Memberships
            .Where(m => accountIds.Contains(m.AccountId))
            .ToListAsync();

        var membershipDict = memberships
            .GroupBy(m => m.AccountId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(m => m.PurchasedAt).First());

        // 5. Lấy tên khu vực
        var regionName = await _db.Propertys
            .Where(l => l.Id == staffLocationId)
            .Select(l => l.Name)
            .FirstOrDefaultAsync() ?? string.Empty;

        // 6. Chuẩn bị kết quả
        var result = new List<PendingMembershipRequestDto>();

        foreach (var r in requests)
        {
            var user = r.UserProfile;
            membershipDict.TryGetValue(r.AccountId, out var membership);

            var interestNames = user?.UserInterests?.Select(i => i.Interest.Name).ToList() ?? new();
            var traitNames = user?.UserPersonalityTraits?.Select(t => t.PersonalityTrait.Name).ToList() ?? new();
            var skillNames = user?.UserSkills?.Select(s => s.Skill.Name).ToList() ?? new();

            result.Add(new PendingMembershipRequestDto
            {
                RequestId = r.Id,
                FullName = user?.FullName,
                RequestedPackageName = r.RequestedPackageName,
                PackageType = r.PackageType,
                Amount = membership?.Amount ?? r.Amount,
                AddOnsFee = r.AddOnsFee,
                ExpireAt = membership?.ExpireAt ?? r.ExpireAt,
                Status = r.Status,
                PaymentStatus = r.PaymentStatus,
                PaymentMethod = membership?.PaymentMethod ?? r.PaymentMethod,
                PaymentTime = membership?.PaymentTime ?? r.PaymentTime,
                StaffNote = r.StaffNote,
                ApprovedAt = r.ApprovedAt,
                MessageToStaff = r.MessageToStaff,
                CreatedAt = r.CreatedAt,
                LocationName = regionName,
                Interests = string.Join(", ", interestNames),
                PersonalityTraits = string.Join(", ", traitNames.Concat(skillNames)),
                Introduction = user?.Introduction,
                CvUrl = user?.CvUrl,
                RequireBooking = r.RequireBooking,
                RoomInstanceId = r.RoomInstanceId,
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

        return result;
    }






    public async Task<BaseResponse> SubmitRequestAsync(Guid accountId, MembershipRequestDto dto)
    {
        if (dto.PackageId == Guid.Empty)
            return BaseResponse.Fail("Missing package ID.");

        var packageType = dto.PackageType?.ToLower();
        if (packageType != "basic" && packageType != "combo")
            return BaseResponse.Fail("Invalid package type. Must be 'basic' or 'combo'.");

        var user = await _db.UserProfiles
            .Include(u => u.UserInterests).ThenInclude(ui => ui.Interest)
            .Include(u => u.UserPersonalityTraits).ThenInclude(pt => pt.PersonalityTrait)
            .Include(u => u.UserSkills).ThenInclude(s => s.Skill)
            .FirstOrDefaultAsync(u => u.AccountId == accountId);

        if (user == null)
            return BaseResponse.Fail("User profile not found.");

        if (!IsUserProfileCompleted(user))
            return BaseResponse.Fail("Please complete your profile before submitting a request.");

        var existingRequest = await _db.PendingMembershipRequests
            .Where(r => r.AccountId == accountId && r.PackageId == dto.PackageId &&
                        (r.Status == "Pending" || r.Status == "PendingPayment" || r.Status == "Completed"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();

        if (existingRequest != null)
        {
            if (existingRequest.Status == "Completed" || existingRequest.Status == "Pending")
                return BaseResponse.Fail("You have already submitted or purchased this package.");

            if (existingRequest.Status == "PendingPayment")
            {
                var elapsed = DateTime.UtcNow - existingRequest.CreatedAt;
                if (elapsed.TotalMinutes < 15)
                    return BaseResponse.Fail("You have already created a request for this package and not completed payment. Please wait 15 minutes or complete the payment.");

                _db.PendingMembershipRequests.Remove(existingRequest);
                await _db.SaveChangesAsync();
            }
        }

        Guid locationId;
        string name;
        decimal price;
        decimal extraFee = 0;

        var selectedStartDate = dto.SelectedStartDate?.Date ?? DateTime.UtcNow.Date;

        // === BASIC PACKAGE ===
        if (packageType == "basic")
        {
            var plan = await _membershipServiceClient.GetBasicPlanByIdAsync(dto.PackageId);
            var duration = await _membershipServiceClient.GetPlanDurationAsync(dto.PackageId, "basic");
            if (plan == null || duration == null || string.IsNullOrWhiteSpace(duration.Unit))
                return BaseResponse.Fail("Unable to get plan or duration info from MembershipService.");

            var priceInfo = await _membershipServiceClient.GetPlanPriceAsync(dto.PackageId, "basic");
            price = Convert.ToDecimal(priceInfo);
            locationId = plan.LocationId ?? Guid.Empty;
            name = plan.Name;

            // ✅ Check Booking Logic
            if (dto.RequireBooking)
            {
                if (dto.RoomInstanceId == null)
                    return BaseResponse.Fail("RoomInstanceId is required.");

                var roomValid = await _membershipServiceClient.IsRoomBelongToPlanAsync(dto.PackageId, dto.RoomInstanceId.Value);
                if (!roomValid)
                    return BaseResponse.Fail("Selected room does not belong to this package.");

                var endDate = CalculateExpireDate(selectedStartDate, duration.Value, duration.Unit);
                var isBooked = await _membershipServiceClient.IsRoomBookedAsync(dto.RoomInstanceId.Value, selectedStartDate, endDate);

                if (isBooked)
                    return BaseResponse.Fail("Room is already booked for the selected date. Please choose another.");

                // ✅ Get Extra Fee from MembershipService
                extraFee = await _membershipServiceClient.GetAddOnFee(dto.RoomInstanceId.Value);
                price += extraFee;
            }

            var request = BuildRequest(accountId, dto.PackageId, name, price, locationId, user, dto.MessageToStaff, "basic", selectedStartDate, extraFee);
            request.PackageDurationValue = duration.Value;
            request.PackageDurationUnit = duration.Unit;
            request.RequireBooking = dto.RequireBooking;
            request.RoomInstanceId = dto.RequireBooking ? dto.RoomInstanceId : null;
            request.AddOnsFee = dto.RequireBooking ? extraFee : null;

            _db.PendingMembershipRequests.Add(request);
            await _db.SaveChangesAsync();

            if (plan.VerifyBuy)
            {
                if (string.IsNullOrWhiteSpace(dto.RedirectUrl))
                    return BaseResponse.Fail("RedirectUrl is required after payment.");

                request.Status = "PendingPayment";
                await _db.SaveChangesAsync();

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
                    return BaseResponse.Fail("Payment creation failed: " + paymentResponse.Message);

                return BaseResponse.Ok("Request created and proceeding to payment.", new
                {
                    IsDirectPurchase = true,
                    RequestId = request.Id,
                    PaymentUrl = paymentResponse.Data
                });
            }

            request.Status = "Pending";
            await _db.SaveChangesAsync();

            return BaseResponse.Ok("Your request has been submitted. Please wait for approval.", new
            {
                IsDirectPurchase = false,
                RequestId = request.Id
            });
        }

        // === COMBO PACKAGE ===
        if (packageType == "combo")
        {
            var combo = await _membershipServiceClient.GetComboPlanByIdAsync(dto.PackageId);
            var duration = await _membershipServiceClient.GetPlanDurationAsync(dto.PackageId, "combo");
            if (combo == null || duration == null || string.IsNullOrWhiteSpace(duration.Unit))
                return BaseResponse.Fail("Unable to get combo plan or duration info.");

            var priceInfo = await _membershipServiceClient.GetPlanPriceAsync(dto.PackageId, "combo");
            price = priceInfo;
            locationId = combo.LocationId ?? Guid.Empty;
            name = combo.Name;

            if (dto.RequireBooking)
            {
                if (dto.RoomInstanceId == null)
                    return BaseResponse.Fail("RoomInstanceId is required.");
                var endDate = CalculateExpireDate(selectedStartDate, duration.Value, duration.Unit);
                var isBooked = await _membershipServiceClient.IsRoomBookedAsync(dto.RoomInstanceId.Value, selectedStartDate, endDate);

                if (isBooked)
                    return BaseResponse.Fail("Room is already booked for the selected date. Please choose another.");

                extraFee = await _membershipServiceClient.GetAddOnFee(dto.RoomInstanceId.Value);
                price += extraFee;
            }

            var request = BuildRequest(accountId, dto.PackageId, name, price, locationId, user, dto.MessageToStaff, "combo", selectedStartDate);
            request.PackageDurationValue = duration.Value;
            request.PackageDurationUnit = duration.Unit;
            request.Status = "Pending";
            request.RequireBooking = dto.RequireBooking;
            request.RoomInstanceId = dto.RequireBooking ? dto.RoomInstanceId : null;
            request.AddOnsFee = dto.RequireBooking ? extraFee : null;

            _db.PendingMembershipRequests.Add(request);
            await _db.SaveChangesAsync();

            return BaseResponse.Ok("Combo plan request submitted successfully.", new
            {
                IsDirectPurchase = false,
                RequestId = request.Id
            });
        }

        return BaseResponse.Fail("Unexpected error occurred.");
    }





    private PendingMembershipRequest BuildRequest(
    Guid accountId,
    Guid packageId,
    string name,
    decimal totalAmount, 
    Guid locationId,
    UserProfile user,
    string? messageToStaff,
    string packageType,
    DateTime startDate,
    decimal? addOnsFee = null 
)
    {
        var traitNames = user.UserPersonalityTraits.Select(p => p.PersonalityTrait.Name);
        var skillNames = user.UserSkills.Select(s => s.Skill.Name);
        var combinedTraits = traitNames.Concat(skillNames);

        return new PendingMembershipRequest
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            PackageId = packageId,
            RequestedPackageName = name ?? "Unknown",
            Amount = totalAmount, 
            AddOnsFee = addOnsFee, 
            LocationId = locationId,
            Interests = string.Join(",", user.UserInterests.Select(i => i.Interest.Name)),
            PersonalityTraits = string.Join(",", combinedTraits),
            Introduction = user.Introduction ?? "",
            CvUrl = user.CvUrl ?? "",
            MessageToStaff = messageToStaff ?? "",
            CreatedAt = DateTime.UtcNow,
            PackageType = packageType ?? "basic",
            StartDate = startDate
        };
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

        // ✅ Cập nhật trạng thái onboarding
        if (request.UserProfile != null)
        {
            request.UserProfile.OnboardingStatus = "ApprovedMember";

            // ✅ CHỈ combo mới nâng role
            if (request.PackageType?.ToLower() == "combo" && request.UserProfile.RoleType != "member")
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
                Console.WriteLine("ℹ️ Gói không yêu cầu nâng role hoặc user đã là member.");
            }
        }

        // ✅ Tính thời hạn sử dụng gói
        int? durationValue = request.PackageDurationValue;
        string? durationUnit = request.PackageDurationUnit;
        DateTime? expireAt = null;

        if (durationValue.HasValue && !string.IsNullOrWhiteSpace(durationUnit))
        {
            expireAt = CalculateExpireDate(paidTime, durationValue.Value, durationUnit);
        }
        var membership = new Membership
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            PackageId = request.PackageId ?? Guid.Empty,
            PackageName = request.RequestedPackageName ?? "Gói không tên",
            PackageType = request.PackageType?.ToLower() ?? "basic",
            Amount = request.Amount ?? 0,
            AddOnsFee = request.AddOnsFee, // ✅ THÊM DÒNG NÀY
            LocationId = request.LocationId ?? Guid.Empty,
            PurchasedAt = paidTime,
            UsedForRoleUpgrade = request.PackageType?.ToLower() == "combo",

            PaymentMethod = request.PaymentMethod,
            PaymentStatus = request.PaymentStatus,
            PaymentNote = request.PaymentNote,
            PaymentTime = request.PaymentTime,
            PaymentTransactionId = request.PaymentTransactionId,

            PackageDurationValue = durationValue,
            PackageDurationUnit = durationUnit,
            ExpireAt = expireAt,
            RoomInstanceId = request.RequireBooking == true ? request.RoomInstanceId : null
        };


        _db.Memberships.Add(membership);
        await _db.SaveChangesAsync();

        // ✅ Tạo booking nếu có yêu cầu và đủ thông tin
        bool bookingCreated = false;

        if (request.RequireBooking == true && request.RoomInstanceId.HasValue)
        {
            try
            {
                if (request.PackageDurationValue.HasValue && !string.IsNullOrWhiteSpace(request.PackageDurationUnit))
                {
                    bookingCreated = await _membershipServiceClient.CreateBookingAsync(
                        accountId: request.AccountId,
                        roomInstanceId: request.RoomInstanceId.Value,
                        startDate: request.StartDate ?? DateTime.UtcNow.Date,
                        durationValue: request.PackageDurationValue.Value,
                        durationUnit: request.PackageDurationUnit
                    );
                }
                else
                {
                    Console.WriteLine("⚠️ Không đủ thông tin thời hạn gói để tạo booking.");
                }

                if (!bookingCreated)
                {
                    Console.WriteLine("⚠️ Đã thanh toán nhưng không thể tạo booking phòng.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🚨 Lỗi tạo booking: {ex.Message}");
            }
        }

        return BaseResponse.Ok("Đã xác nhận thanh toán và cập nhật Membership thành công.", new
        {
            MembershipId = membership.Id,
            PackageName = membership.PackageName,
            PackageType = membership.PackageType,
            Upgraded = membership.UsedForRoleUpgrade,
            ExpireAt = membership.ExpireAt,
            BookingCreated = bookingCreated
        });
    }






    public DateTime CalculateExpireDate(DateTime start, int value, string unit)
    {
        return unit.ToLower() switch
        {
            "day" => start.AddDays(value),
            "month" => start.AddMonths(value),
            "year" => start.AddYears(value),
            _ => start
        };
    }

    bool IMembershipRequestService.IsUserProfileCompleted(UserProfile user)
    {
        return !string.IsNullOrWhiteSpace(user.FullName)
         && !string.IsNullOrWhiteSpace(user.Phone)
         && !string.IsNullOrWhiteSpace(user.Gender)
         && user.DOB.HasValue
         && !string.IsNullOrWhiteSpace(user.AvatarUrl)
         && user.UserInterests != null && user.UserInterests.Any()
         && ((user.UserPersonalityTraits != null && user.UserPersonalityTraits.Any())
             || (user.UserSkills != null && user.UserSkills.Any()))
         && !string.IsNullOrWhiteSpace(user.Introduction)
         && !string.IsNullOrWhiteSpace(user.CvUrl);
    }

    public async Task<BaseResponse> CancelRequestAsync(Guid accountId, Guid requestId)
    {
       
        var request = await _db.PendingMembershipRequests
            .FirstOrDefaultAsync(r => r.Id == requestId && r.AccountId == accountId);

        if (request == null)
            return BaseResponse.Fail("Yêu cầu không tồn tại hoặc không thuộc quyền của bạn.");

      
        if (request.Status != PendingRequestStatus.Pending &&
            request.Status != PendingRequestStatus.PendingPayment)
        {
            return BaseResponse.Fail("Chỉ có thể hủy yêu cầu khi đang chờ xử lý hoặc chờ thanh toán.");
        }

        _db.PendingMembershipRequests.Remove(request);
        await _db.SaveChangesAsync();

        return BaseResponse.Ok("Hủy yêu cầu thành công.");
    }



}
