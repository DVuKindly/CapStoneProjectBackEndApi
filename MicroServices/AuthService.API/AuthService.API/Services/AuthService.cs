using AuthService.API.DTOs.AdminCreate;

using AuthService.API.DTOs.COACH;
using AuthService.API.DTOs.PARTNER;
using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using AuthService.API.DTOs.STAFF;
using AuthService.API.DTOs.SUPPLIER;
using AuthService.API.Entities;
using AuthService.API.Helpers;
using AuthService.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IEmailService emailService,
            IUserServiceClient userServiceClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = new PasswordHasher<UserAuth>();
            _userServiceClient = userServiceClient;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "Tài khoản không tồn tại." };
            }

            if (!user.EmailVerified)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email chưa được xác minh. Vui lòng kiểm tra email để xác thực tài khoản."
                };
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return new AuthResponse { Success = false, Message = "Sai mật khẩu." };
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            if (user.IsLocked)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên."
                };
            }

            return new AuthResponse
            {
                Success = true,
                Email = user.Email,
                FullName = user.UserName,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string token)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(token);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                return new AuthResponse { Success = false, Message = "Invalid or expired refresh token." };

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userRepository.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                Email = user.Email,
                FullName = user.UserName,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
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
                return new AuthResponse { Success = false, Message = "Email không tồn tại." };
            }

            user.ResetPasswordToken = Guid.NewGuid().ToString();
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(2);

            await _userRepository.SaveChangesAsync();
            var resetPasswordUrl = $"http://localhost:3000/reset-password?token={user.ResetPasswordToken}";
            await _emailService.SendResetPasswordEmailAsync(user.Email, user.ResetPasswordToken!);

            return new AuthResponse { Success = true, Message = "Đã gửi email để reset mật khẩu." };
        }


        public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userRepository.GetByResetPasswordTokenAsync(request.Token);
            if (user == null || user.ResetPasswordTokenExpiry < DateTime.UtcNow)
            {
                return new AuthResponse { Success = false, Message = "Token không hợp lệ hoặc đã hết hạn." };
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync();

            return new AuthResponse { Success = true, Message = "Mật khẩu đã được thay đổi thành công." };
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

        public async Task<AuthResponse> GoogleLoginAsync(GoogleLoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                user = new UserAuth
                {
                    UserId = Guid.NewGuid(),
                    Email = request.Email,
                    UserName = request.FullName,
                    Provider = "Google",
                    ProviderId = request.ProviderId,
                    PasswordHash = null,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();
            }

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _userRepository.SaveChangesAsync();

            return new AuthResponse
            {
                Success = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Email = user.Email,
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
                return new AuthResponse { Success = false, Message = "Email đã tồn tại." };
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
                return new AuthResponse { Success = false, Message = "Role 'User' không tồn tại." };
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
            Note = "Tạo từ RegisterAsync"
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
                Message = "Đăng ký thành công. Vui lòng xác minh email để tiếp tục.",
                AccessToken = string.Empty,
                RefreshToken = string.Empty
            };
        }




        public async Task<AuthResponse> RegisterAdminAsync(RegisterBySuperAdminRequest request)
        {
            // ✅ Kiểm tra LocationId hợp lệ bằng UserService
            var isValidLocation = await _userServiceClient.IsValidLocationAsync(request.LocationId);
            if (!isValidLocation)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Khu vực không hợp lệ. Vui lòng chọn từ danh sách được hỗ trợ."
                };
            }

            // 🔍 Kiểm tra email đã tồn tại chưa
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return new AuthResponse { Success = false, Message = "Email đã tồn tại." };

            // 🧾 Tạo user admin
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

            // 🔑 Gán role "admin"
            var role = await _userRepository.GetRoleByKeyAsync("admin");
            if (role == null)
                return new AuthResponse { Success = false, Message = "Vai trò không hợp lệ." };

            user.UserRoles = new List<UserRole> { new UserRole { UserId = user.UserId, RoleId = role.RoleId } };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // 👤 Ai là người tạo?
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? createdByAdminId = Guid.TryParse(currentUserId, out var parsed) ? parsed : null;

            // 👑 Nếu là super_admin thì gắn AdminSystem
            var currentUserRole = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
            var onboardingStatus = currentUserRole == "super_admin" ? "AdminSystem" : "AdminLocation";

            // 📦 Gửi profile sang UserService
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

            // ✉️ Gửi email xác minh nếu cần
            if (!request.SkipEmailVerification && user.EmailVerificationToken != null)
                await _emailService.SendVerificationEmailAsync(user.Email, user.EmailVerificationToken);

            // ✅ Trả kết quả
            return new AuthResponse
            {
                Success = true,
                Email = user.Email,
                FullName = user.UserName,
                Message = request.SkipEmailVerification
                    ? "Admin đã được tạo và xác minh."
                    : "Admin đã được tạo. Vui lòng xác minh email.",
                AccessToken = "",
                RefreshToken = ""
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
            // 🔐 Lấy location và role hiện tại từ token
            var currentUserLocation = _httpContextAccessor.HttpContext?.User?.FindFirst("location")?.Value;
            var currentUserRole = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

            // ✅ SuperAdmin được bỏ qua kiểm tra, Admin phải kiểm tra khu vực
            if (currentUserRole == "admin")
            {
                if (string.IsNullOrEmpty(currentUserLocation) || currentUserLocation != request.LocationId.ToString())
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Admin chỉ được phép tạo tài khoản trong khu vực của mình."
                    };
                }
            }

            // ✅ Kiểm tra khu vực có hợp lệ không
            var isValidLocation = await _userServiceClient.IsValidLocationAsync(request.LocationId);
            if (!isValidLocation)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Khu vực không hợp lệ. Vui lòng chọn từ danh sách được hỗ trợ."
                };
            }

            // 🔍 Kiểm tra email đã tồn tại chưa
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return new AuthResponse { Success = false, Message = "Email đã tồn tại." };

            // 🔑 Chuẩn bị token đặt mật khẩu
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

            // 🧾 Gán role
            var role = await _userRepository.GetRoleByKeyAsync(request.RoleKey);
            if (role == null)
                return new AuthResponse { Success = false, Message = "Vai trò không hợp lệ." };

            user.UserRoles = new List<UserRole>
    {
        new UserRole { UserId = user.UserId, RoleId = role.RoleId }
    };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // 👤 Lấy ID người tạo
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? createdByAdminId = Guid.TryParse(currentUserId, out var parsedGuid) ? parsedGuid : null;

            // 📦 Tạo profile
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
                    return new AuthResponse { Success = false, Message = "Loại hồ sơ không được hỗ trợ." };
            }

            await _userServiceClient.CreateUserProfileAsync(user.UserId, user.UserName, user.Email, request.RoleKey, profile);

            // 📧 Gửi email đặt mật khẩu
            await _emailService.SendSetPasswordEmailAsync(user.Email, resetToken);

            return new AuthResponse
            {
                Success = true,
                Email = user.Email,
                FullName = user.UserName,
                Message = $"Tài khoản {request.RoleKey} đã được tạo. Vui lòng kiểm tra email để thiết lập mật khẩu.",
                AccessToken = string.Empty,
                RefreshToken = string.Empty
            };
        }









        public async Task<AuthResponse> SetPasswordAsync(SetPasswordThirtyRequest request)
        {
            var user = await _userRepository.GetByResetPasswordTokenAsync(request.Token); // ✅ tìm theo token

            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "Token không hợp lệ hoặc không tồn tại." };
            }

            if (user.ResetPasswordTokenExpiry.HasValue && user.ResetPasswordTokenExpiry.Value < DateTime.UtcNow)
            {
                return new AuthResponse { Success = false, Message = "Token đã hết hạn." };
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
                Message = "Mật khẩu đã được thiết lập thành công. Bạn có thể đăng nhập ngay bây giờ."
            };
        }

        public async Task<List<LocationDto>> GetLocationsAsync()
        {
            return await _userServiceClient.GetLocationsAsync();
        }
    }
}
