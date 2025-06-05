using AuthService.API.DTOs.Request;
using AuthService.API.DTOs.Responses;
using AuthService.API.Entities;
using AuthService.API.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthService.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IPasswordHasher<UserAuth> _passwordHasher;
        private readonly IUserServiceClient _userServiceClient;

        public AuthService(
            IUserRepository userRepository,
            ITokenService tokenService,
            IEmailService emailService,
            IUserServiceClient userServiceClient)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _passwordHasher = new PasswordHasher<UserAuth>();
            _userServiceClient = userServiceClient;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Mật khẩu không được để trống."
                };
            }

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
                EmailVerificationToken = Guid.NewGuid().ToString(),
                EmailVerificationExpiry = DateTime.UtcNow.AddHours(24),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EmailVerified = false,
                IsLocked = false,
            };

            var guestRole = await _userRepository.GetRoleByKeyAsync("User");
            user.UserRoles = new List<UserRole>
            {
                new UserRole
                {
                    UserId = user.UserId,
                    RoleId = guestRole.RoleId
                }
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            await _userServiceClient.CreateUserProfileAsync(
        user.UserId, 
        user.UserName,
        user.Email
    );

            await _emailService.SendVerificationEmailAsync(user.Email, user.EmailVerificationToken!);

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
    }
}
