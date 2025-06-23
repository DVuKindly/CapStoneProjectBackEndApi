using AuthService.API.Data;
using AuthService.API.DTOs;
using AuthService.API.DTOs.AdminCreate;

using AuthService.API.DTOs.COACH;
using AuthService.API.DTOs.PARTNER;
using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using AuthService.API.DTOs.STAFF;
using AuthService.API.DTOs.SUPPLIER;
using AuthService.API.Entities;
using AuthService.API.Helpers;
using AuthService.API.Helpers.AuthService.API.Helpers;
using AuthService.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuthService.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher<UserAuth> _passwordHasher;
        private readonly IUserServiceClient _userServiceClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _jwtSettings;
        private readonly AuthDbContext _context;
        


        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IEmailService emailService,
            IOptions<JwtSettings> jwtOptions,
             AuthDbContext context,
            IUserServiceClient userServiceClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _context = context;
            _jwtSettings = jwtOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = new PasswordHasher<UserAuth>();
            _userServiceClient = userServiceClient;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // ⚠️ Query user and include their roles
            var user = await _userRepository.GetByEmailWithRoleAsync(request.Email);

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.AccountNotFound
                };
            }

            if (!user.EmailVerified)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.EmailNotVerified
                };
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.LoginFailed
                };
            }

            if (user.IsLocked)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.AccountLocked
                };
            }

            // ✅ Generate tokens
            var permissionKeys = user.UserRoles
     .SelectMany(ur => ur.Role.RolePermissions)
     .Select(rp => rp.Permission.PermissionKey)
     .Distinct()
     .ToList();

            var accessToken = _tokenService.GenerateAccessToken(user, permissionKeys);

            var refreshToken = _tokenService.GenerateRefreshToken();
            var idToken = _tokenService.GenerateIdToken(user);

            // ✅ Update refresh token info
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            var roleKey = user.UserRoles?.FirstOrDefault()?.Role?.RoleKey;

            return new AuthResponse
            {
                UserId = user.UserId,
                LocationId = user.LocationId,
                Success = true,
                Message = AuthMessages.LoginSuccess,
                Email = user.Email,
                FullName = user.UserName,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IdToken = idToken,
                Role = roleKey
            };
        }

        public async Task<BaseResponse> ChangeUserRoleAsync(Guid userId, Guid newRoleId)
        {
            var user = await _userRepository.GetUserWithRolesByAccountIdAsync(userId);
            if (user == null)
                return new BaseResponse(false, "User not found");

            // Xoá toàn bộ role cũ
            foreach (var oldRole in user.UserRoles)
            {
                await _userRepository.RemoveUserRoleAsync(user.UserId, oldRole.RoleId);
            }

            // Gán role mới
            var newUserRole = new UserRole
            {
                UserId = userId,
                RoleId = newRoleId
            };

            await _userRepository.AddUserRoleAsync(newUserRole);
            await _userRepository.SaveChangesAsync();

            return new BaseResponse(true, "User role updated successfully");
        }



        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = await _context.AuthUsers
                .Include(u => u.UserRoles).ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            // ❌ Nếu không tìm thấy hoặc token hết hạn
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid or expired refresh token."
                };
            }

            // ✅ Ngăn dùng lại token cũ bằng cách xóa
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _context.SaveChangesAsync();

            // ✅ Sinh token mới
            var permissionKeys = user.UserRoles
       .SelectMany(ur => ur.Role.RolePermissions)
       .Select(rp => rp.Permission.PermissionKey)
       .Distinct()
       .ToList();

            var newAccessToken = _tokenService.GenerateAccessToken(user, permissionKeys);

            var newIdToken = _tokenService.GenerateIdToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newRefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDays);

            // ✅ Gán token mới
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = newRefreshTokenExpiry;
            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                IdToken = newIdToken,
                Email = user.Email,
                FullName = user.UserName,
                Role = user.UserRoles.FirstOrDefault()?.Role?.RoleKey ?? string.Empty,
                UserId = user.UserId,
                LocationId = user.LocationId
            };
        }



        public async Task<AuthResponse> ChangePasswordAsync(ChangePasswordRequest request, string token)
        {
            try
            {

                var userPrincipal = _tokenService.GetPrincipalFromExpiredToken(token);
                if (userPrincipal == null)
                {
                    return new AuthResponse { Success = false, Message = "Invalid or expired token." };
                }

                var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return new AuthResponse { Success = false, Message = "User ID not found in token." };
                }

                var user = await _userRepository.GetByIdAsync(Guid.Parse(userIdClaim.Value));
                if (user == null)
                {
                    return new AuthResponse { Success = false, Message = "User not found." };
                }

                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword);
                if (result == PasswordVerificationResult.Failed)
                {
                    return new AuthResponse { Success = false, Message = "Invalid old password." };
                }

                user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;

                await _userRepository.SaveChangesAsync();

                return new AuthResponse { Success = true, Message = "Password changed successfully." };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }


        public async Task<AuthResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.EmailNotFound
                };
            }

            user.ResetPasswordToken = Guid.NewGuid().ToString();
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(2);

            await _userRepository.SaveChangesAsync();

            var resetPasswordUrl = $"http://localhost:3000/reset-password?token={user.ResetPasswordToken}";
            await _emailService.SendResetPasswordEmailAsync(user.Email, user.ResetPasswordToken!);

            return new AuthResponse
            {
                Success = true,
                Message = AuthMessages.ResetEmailSent
            };
        }



        public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userRepository.GetByResetPasswordTokenAsync(request.Token);
            if (user == null || user.ResetPasswordTokenExpiry < DateTime.UtcNow)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.InvalidOrExpiredToken
                };
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                Message = AuthMessages.PasswordResetSuccess
            };
        }



        public async Task<bool> VerifyEmailAsync(string token)
        {
            var user = await _userRepository.GetByEmailVerificationTokenAsync(token);
            if (user == null || user.EmailVerificationExpiry < DateTime.UtcNow) return false;

            user.EmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<AuthStatusResponse> GetStatusAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
            if (user == null) return new AuthStatusResponse { Exists = false };

            return new AuthStatusResponse
            {
                Exists = true,
                EmailVerified = user.EmailVerified,
                IsLocked = user.IsLocked,
                FullName = user.UserName
            };
        }

       


        public async Task<AuthResponse> LogoutAsync(string token)
        {

            var userPrincipal = _tokenService.GetPrincipalFromExpiredToken(token);
            if (userPrincipal == null)
            {
                return new AuthResponse { Success = false, Message = "Invalid token." };
            }

            var userIdClaim = userPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return new AuthResponse { Success = false, Message = "User not found." };
            }

            var user = await _userRepository.GetByIdAsync(Guid.Parse(userIdClaim.Value));
            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "User not found." };
            }


            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _userRepository.SaveChangesAsync();

            return new AuthResponse { Success = true, Message = "Logged out successfully." };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.EmailAlreadyExists
                };
            }

            var user = new UserAuth
            {
                UserId = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(null!, request.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsLocked = false,
                EmailVerified = false,
                EmailVerificationToken = Guid.NewGuid().ToString(),
                EmailVerificationExpiry = DateTime.UtcNow.AddHours(24)
            };

            var role = await _userRepository.GetRoleByKeyAsync("user");
            if (role == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.RoleNotFound
                };
            }

            user.UserRoles = new List<UserRole>
    {
        new UserRole
        {
            UserId = user.UserId,
            RoleId = role.RoleId
        }
    };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            await _userServiceClient.CreateUserProfileAsync(
                user.UserId,
                user.UserName,
                user.Email,
                "user",
                new UserProfilePayload
                {
                    AccountId = user.UserId,
                    FullName = user.UserName,
                    Email = user.Email,
                    RoleType = "user",
                    OnboardingStatus = "Pending",
                    Note = "Created from RegisterAsync"
                }
            );

            if (!string.IsNullOrEmpty(user.EmailVerificationToken))
            {
                await _emailService.SendVerificationEmailAsync(user.Email, user.EmailVerificationToken);
            }

            return new AuthResponse
            {
                Success = true,
                Email = user.Email,
                FullName = user.UserName,
                Message = AuthMessages.RegisterSuccess,
                AccessToken = string.Empty,
                RefreshToken = string.Empty
            };
        }





        public async Task<AuthResponse> RegisterAdminAsync(RegisterBySuperAdminRequest request)
        {
            // ✅ Validate location via UserService
            var isValidLocation = await _userServiceClient.IsValidLocationAsync(request.LocationId);
            if (!isValidLocation)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.InvalidLocation
                };
            }

            // 🔍 Check if email already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.AdminAlreadyExists
                };
            }

            // 🧾 Create user
            var user = new UserAuth
            {
                UserId = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(null!, request.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsLocked = false,
                EmailVerified = request.SkipEmailVerification,
                EmailVerificationToken = request.SkipEmailVerification ? null : Guid.NewGuid().ToString(),
                EmailVerificationExpiry = request.SkipEmailVerification ? null : DateTime.UtcNow.AddHours(24),
                LocationId = request.LocationId
            };

            // 🔑 Assign role "admin"
            var role = await _userRepository.GetRoleByKeyAsync("admin");
            if (role == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.RoleInvalid
                };
            }

            user.UserRoles = new List<UserRole>
    {
        new UserRole { UserId = user.UserId, RoleId = role.RoleId }
    };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // 👤 Get creator info
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? createdByAdminId = Guid.TryParse(currentUserId, out var parsed) ? parsed : null;

            // 👑 Determine onboarding status
            var currentUserRole = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
            var onboardingStatus = currentUserRole == "super_admin" ? "AdminSystem" : "AdminLocation";

            // 📦 Send profile to UserService
            var profile = new UserProfilePayload
            {
                AccountId = user.UserId,
                FullName = user.UserName,
                Email = user.Email,
                RoleType = "admin",
                LocationId = request.LocationId,
                OnboardingStatus = onboardingStatus,
                Note = "CreatedBySuperAdmin",
                CreatedByAdminId = createdByAdminId
            };

            await _userServiceClient.CreateUserProfileAsync(user.UserId, user.UserName, user.Email, "admin", profile);

            // ✉️ Send email verification if needed
            if (!request.SkipEmailVerification && user.EmailVerificationToken != null)
            {
                await _emailService.SendVerificationEmailAsync(user.Email, user.EmailVerificationToken);
            }

            
            return new AuthResponse
            {
                Success = true,
                Email = user.Email,
                FullName = user.UserName,
                Message = request.SkipEmailVerification
                    ? AuthMessages.AdminCreatedAndVerified
                    : AuthMessages.AdminCreatedNeedVerify,
                AccessToken = string.Empty,
                RefreshToken = string.Empty
            };
        }






        public async Task<AuthResponse> RegisterManagerAsync(RegisterStaffRequest request)
        {


            return await RegisterSystemAccountAsync(new AdminAccountRegisterAdapter
            {
                UserName = request.UserName,
                Email = request.Email,
                SkipPasswordCreation = true,           
                SkipEmailVerification = false,
                RoleKey = "manager",
                LocationId = request.LocationId,
             
                ProfileInfo = request.ProfileInfo
            });
        }





        public async Task<AuthResponse> RegisterStaffAsync(RegisterStaffRequest request)
        {
            return await RegisterSystemAccountAsync(new AdminAccountRegisterAdapter
            {
                UserName = request.UserName,
                Email = request.Email,
                SkipPasswordCreation = true,          
                SkipEmailVerification = false,
                RoleKey = request.RoleKey!, 
                LocationId = request.LocationId,
                ProfileInfo = request.ProfileInfo
            });
        }

        public async Task<AuthResponse> RegisterCoachAsync(RegisterCoachRequest request)
        {
            return await RegisterSystemAccountAsync(new AdminAccountRegisterAdapter
            {
                UserName = request.UserName,
                Email = request.Email,
                SkipPasswordCreation = true,           
                SkipEmailVerification = false,
                RoleKey = "coaching",
                LocationId = request.LocationId,
                ProfileInfo = request.ProfileInfo
            });
        }

        public async Task<AuthResponse> RegisterPartnerAsync(RegisterPartnerRequest request)
        {
            return await RegisterSystemAccountAsync(new AdminAccountRegisterAdapter
            {
                UserName = request.UserName,
                Email = request.Email,
                SkipPasswordCreation = true,           // ✅ KHÔNG có mật khẩu
                SkipEmailVerification = false,
                RoleKey = "partner",
                LocationId = request.LocationId,
                 ProfileInfo = request.ProfileInfo
            });
        }

        public async Task<AuthResponse> RegisterSupplierAsync(RegisterSupplierRequest request)
        {
            return await RegisterSystemAccountAsync(new AdminAccountRegisterAdapter
            {
                UserName = request.UserName,
                Email = request.Email,
                SkipPasswordCreation = true,           // ✅ KHÔNG có mật khẩu
                SkipEmailVerification = false,
                RoleKey = "supplier",
                LocationId = request.LocationId,
               ProfileInfo = request.ProfileInfo
            });
        }




        public async Task<AuthResponse> RegisterSystemAccountAsync(AdminAccountRegisterAdapter request)
        {
            var currentUserLocation = _httpContextAccessor.HttpContext?.User?.FindFirst("location")?.Value;
            var currentUserRole = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

            if (currentUserRole == "admin")
            {
                if (string.IsNullOrEmpty(currentUserLocation) || currentUserLocation != request.LocationId.ToString())
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = AuthMessages.UnauthorizedLocationCreation
                    };
                }
            }

            var isValidLocation = await _userServiceClient.IsValidLocationAsync(request.LocationId);
            if (!isValidLocation)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.InvalidLocation
                };
            }

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.RoleAlreadyExists
                };
            }

            var resetToken = Guid.NewGuid().ToString();

            var user = new UserAuth
            {
                UserId = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LocationId = request.LocationId,
                IsLocked = false,
                EmailVerified = true,
                EmailVerificationToken = null,
                EmailVerificationExpiry = null,
                ResetPasswordToken = resetToken,
                ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(24)
            };

            var role = await _userRepository.GetRoleByKeyAsync(request.RoleKey);
            if (role == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.RoleInvalid
                };
            }

            user.UserRoles = new List<UserRole>
    {
        new UserRole { UserId = user.UserId, RoleId = role.RoleId }
    };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? createdByAdminId = Guid.TryParse(currentUserId, out var parsedGuid) ? parsedGuid : null;

            var profile = new UserProfilePayload
            {
                AccountId = user.UserId,
                FullName = user.UserName,
                Email = user.Email,
                RoleType = request.RoleKey,
                CreatedByAdminId = createdByAdminId,
                LocationId = request.LocationId
            };

            if (request.RoleKey == "admin")
                profile.OnboardingStatus = "AdminLocation";

            switch (request.ProfileInfo)
            {
                case StaffProfileInfoRequest staff:
                    profile.Phone = staff.Phone;
                    profile.Gender = staff.Gender;
                    profile.DOB = staff.DOB?.ToString("yyyy-MM-dd");
                    profile.Note = staff.Note;
                    profile.Department = staff.Department;
                    profile.Level = staff.Level;
                    break;

                case CoachProfileInfoRequest coach:
                    profile.CoachType = coach.CoachType;
                    profile.Specialty = coach.Specialty;
                    profile.Module = coach.ModuleInCharge;
                    profile.Note = coach.Bio;
                    profile.Certifications = coach.Certifications;
                    profile.LinkedInUrl = coach.LinkedInUrl;
                    break;

                case PartnerProfileInfoRequest partner:
                    profile.OrganizationName = partner.OrganizationName;
                    profile.PartnerType = partner.PartnerType;
                    profile.Note = partner.Note;
                    profile.RepresentativeName = partner.RepresentativeName;
                    profile.RepresentativePhone = partner.RepresentativePhone;
                    profile.RepresentativeEmail = partner.RepresentativeEmail;
                    profile.Description = partner.Description;
                    profile.WebsiteUrl = partner.WebsiteUrl;
                    profile.Industry = partner.Industry;
                    break;

                case SupplierProfileInfoRequest supplier:
                    profile.OrganizationName = supplier.CompanyName;
                    profile.RepresentativeName = supplier.ContactPerson;
                    profile.RepresentativePhone = supplier.ContactPhone;
                    profile.RepresentativeEmail = supplier.ContactEmail;
                    profile.Description = supplier.Description;
                    profile.WebsiteUrl = supplier.WebsiteUrl;
                    profile.Industry = supplier.Industry;
                    profile.Note = supplier.TaxCode;
                    break;

                default:
                    return new AuthResponse
                    {
                        Success = false,
                        Message = AuthMessages.UnsupportedProfileInfo
                    };
            }

            await _userServiceClient.CreateUserProfileAsync(user.UserId, user.UserName, user.Email, request.RoleKey, profile);

            await _emailService.SendSetPasswordEmailAsync(user.Email, resetToken);

            return new AuthResponse
            {
                Success = true,
                Email = user.Email,
                FullName = user.UserName,
                Message = string.Format(AuthMessages.SystemAccountCreated, request.RoleKey),
                AccessToken = string.Empty,
                RefreshToken = string.Empty
            };
        }










        public async Task<AuthResponse> SetPasswordAsync(SetPasswordThirtyRequest request)
        {
            var user = await _userRepository.GetByResetPasswordTokenAsync(request.Token);

            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.InvalidToken
                };
            }

            if (user.ResetPasswordTokenExpiry.HasValue && user.ResetPasswordTokenExpiry.Value < DateTime.UtcNow)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = AuthMessages.TokenExpired
                };
            }

            user.PasswordHash = _passwordHasher.HashPassword(null!, request.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                Email = user.Email,
                FullName = user.UserName,
                Message = AuthMessages.SetPasswordSuccess
            };
        }


        public async Task<List<LocationDto>> GetLocationsAsync()
        {
            return await _userServiceClient.GetLocationsAsync();
        }

        public async Task<bool> PromoteUserToMemberAsync(Guid accountId)
        {
            var user = await _userRepository.GetUserWithRolesByAccountIdAsync(accountId);
            if (user == null)
                return false;

            var memberRole = await _userRepository.GetRoleByKeyAsync("member");
            if (memberRole == null)
                return false;

            // Xóa role cũ
            if (user.UserRoles != null && user.UserRoles.Any())
            {
                foreach (var ur in user.UserRoles.ToList())
                {
                    await _userRepository.RemoveUserRoleAsync(user.UserId, ur.RoleId);
                }
            }

            // Thêm role member
            await _userRepository.AddUserRoleAsync(new UserRole
            {
                UserId = user.UserId,
                RoleId = memberRole.RoleId
            });

            // Cập nhật profile bên UserService
            var profileUpdate = new UserProfilePayload
            {
                AccountId = user.UserId,
                OnboardingStatus = "Approved",
                RoleType = "member"
            };

            await _userServiceClient.UpdateUserProfileStatusAsync(profileUpdate);

            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public Task<AuthResponse> GoogleLoginAsync(GoogleLoginRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AssignRoleAsync(Guid userId, Guid roleId)
        {
            var exists = await  _context.UserRoles.AnyAsync(x => x.UserId == userId && x.RoleId == roleId);
            if (exists)
                return new BaseResponse(false, "User already has this role.");

             _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = roleId });
            await  _context.SaveChangesAsync();
            return new BaseResponse(true, "Role assigned successfully.");
        }

        public async Task<BaseResponse> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId)
        {
            var exists = await  _context.RolePermissions.AnyAsync(x => x.RoleId == roleId && x.PermissionId == permissionId);
            if (exists)
                return new BaseResponse(false, "Permission already assigned to this role.");

             _context.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = permissionId });
            await  _context.SaveChangesAsync();
            return new BaseResponse(true, "Permission assigned to role successfully.");
        }

        public async Task<BaseResponse> AssignPermissionToUserAsync(Guid userId, Guid permissionId)
        {
            var exists = await  _context.UserPermissions.AnyAsync(x => x.UserId == userId && x.PermissionId == permissionId);
            if (exists)
                return new BaseResponse(false, "Permission already assigned to user.");

             _context.UserPermissions.Add(new UserPermission { UserId = userId, PermissionId = permissionId });
            await  _context.SaveChangesAsync();
            return new BaseResponse(true, "Permission assigned to user successfully.");
        }

        public async Task<List<RoleDto>> GetUserRolesAsync(Guid userId)
        {
            var roles = await  _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role)
                .Select(ur => new RoleDto
                {
                    RoleId = ur.Role.RoleId,
                    RoleKey = ur.Role.RoleKey,
                    RoleName = ur.Role.RoleName,
                    Description = ur.Role.Description
                })
                .ToListAsync();

            return roles;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            return await  _context.Roles.Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleKey = r.RoleKey,
                RoleName = r.RoleName,
                Description = r.Description
            }).ToListAsync();
        }

        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            return await  _context.Permissions.Select(p => new PermissionDto
            {
                PermissionId = p.PermissionId,
                PermissionKey = p.PermissionKey,
                Description = p.Description
            }).ToListAsync();
        }
    }
}
