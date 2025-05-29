using System.Net.Http.Json;
using BffService.API.DTOs.Auth.Request;
using BffService.API.DTOs.Auth.Response;

using Microsoft.Extensions.Configuration;

namespace BffService.API.Services
{
    public class AuthProxyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _authServiceBaseUrl;

        public AuthProxyService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authServiceBaseUrl = configuration["Services:AuthService"]
                                  ?? throw new InvalidOperationException("AuthService base URL not configured.");
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_authServiceBaseUrl}/api/auth/register", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_authServiceBaseUrl}/api/auth/login", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_authServiceBaseUrl}/api/auth/refresh", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<BaseResponse?> VerifyEmailAsync(VerifyEmailRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_authServiceBaseUrl}/api/auth/verify-email", request);
            return await response.Content.ReadFromJsonAsync<BaseResponse>();
        }

        public async Task<BaseResponse?> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_authServiceBaseUrl}/api/auth/forgot-password", request);
            return await response.Content.ReadFromJsonAsync<BaseResponse>();
        }

        public async Task<BaseResponse?> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_authServiceBaseUrl}/api/auth/reset-password", request);
            return await response.Content.ReadFromJsonAsync<BaseResponse>();
        }

        public async Task<AuthResponse?> GoogleLoginAsync(GoogleLoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_authServiceBaseUrl}/api/auth/google-login", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<AuthStatusResponse?> GetStatusAsync(string userId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_authServiceBaseUrl}/api/auth/status/{userId}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<AuthStatusResponse>();
        }
    }
}
