// File: BffService.API/Services/AuthProxyService.cs
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BffService.API.DTOs.AdminCreate;
using BffService.API.DTOs.Request;
using BffService.API.DTOs.Responses;

namespace BffService.API.Services
{
    public class AuthProxyService : IAuthProxyService
    {
        private readonly HttpClient _httpClient;

        public AuthProxyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
            => await (await _httpClient.PostAsJsonAsync("/api/auth/register", request)).Content.ReadFromJsonAsync<AuthResponse>();

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
            => await (await _httpClient.PostAsJsonAsync("/api/auth/login", request)).Content.ReadFromJsonAsync<AuthResponse>();

        public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request)
            => await (await _httpClient.PostAsJsonAsync("/api/auth/refresh", request)).Content.ReadFromJsonAsync<AuthResponse>();

        public async Task<BaseResponse?> VerifyEmailAsync(VerifyEmailRequest request)
            => await (await _httpClient.PostAsJsonAsync("/api/auth/verify-email", request)).Content.ReadFromJsonAsync<BaseResponse>();

        public async Task<AuthResponse?> ForgotPasswordAsync(ForgotPasswordRequest request)
            => await (await _httpClient.PostAsJsonAsync("/api/auth/forgot-password", request)).Content.ReadFromJsonAsync<AuthResponse>();

        public async Task<AuthResponse?> ResetPasswordAsync(ResetPasswordRequest request)
            => await (await _httpClient.PostAsJsonAsync("/api/auth/reset-password", request)).Content.ReadFromJsonAsync<AuthResponse>();

        public async Task<AuthResponse?> GoogleLoginAsync(GoogleLoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/google-login", request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AuthResponse>()
                : new AuthResponse { Success = false, Message = await response.Content.ReadAsStringAsync() };
        }

        public async Task<AuthStatusResponse?> GetStatusAsync(string userId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/auth/status/{userId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<AuthStatusResponse>();
        }

        public async Task<AuthResponse?> LogoutAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/logout");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AuthResponse>()
                : new AuthResponse { Success = false, Message = await response.Content.ReadAsStringAsync() };
        }

        public async Task<AuthResponse?> ChangePasswordAsync(ChangePasswordRequest request, string token)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/api/auth/change-password")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
                Content = JsonContent.Create(request)
            };
            var response = await _httpClient.SendAsync(httpRequest);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AuthResponse>()
                : new AuthResponse { Success = false, Message = await response.Content.ReadAsStringAsync() };
        }

        public async Task<AuthResponse?> SetPasswordAsync(SetPasswordThirtyRequest request)
            => await (await _httpClient.PostAsJsonAsync("/api/auth/set-passwordthirtytoken", request)).Content.ReadFromJsonAsync<AuthResponse>();

        public async Task<AuthResponse?> RegisterAdminAsync(RegisterBySuperAdminRequest request, string token)
            => await SendAuthorizedPost<AuthResponse>("/api/auth/register/admin", request, token);

        public async Task<AuthResponse?> RegisterManagerAsync(RegisterStaffRequest request, string token)
            => await SendAuthorizedPost<AuthResponse>("/api/auth/register/manager", request, token);

        public async Task<AuthResponse?> RegisterCoachAsync(RegisterCoachRequest request, string token)
            => await SendAuthorizedPost<AuthResponse>("/api/auth/register/coach", request, token);

        public async Task<AuthResponse?> RegisterPartnerAsync(RegisterPartnerRequest request, string token)
            => await SendAuthorizedPost<AuthResponse>("/api/auth/register/partner", request, token);

        public async Task<AuthResponse?> RegisterSupplierAsync(RegisterSupplierRequest request, string token)
            => await SendAuthorizedPost<AuthResponse>("/api/auth/register/supplier", request, token);

        public async Task<AuthResponse?> RegisterStaffAsync(RegisterStaffRequest request, string token)
        {
            var endpoint = request.RoleKey switch
            {
                "staff_onboarding" => "/api/auth/register/staff-onboarding",
                "staff_service" => "/api/auth/register/staff-service",
                "staff_content" => "/api/auth/register/staff-content",
                _ => throw new ArgumentException("Invalid RoleKey for staff.")
            };

            return await SendAuthorizedPost<AuthResponse>(endpoint, request, token);
        }

        private async Task<T?> SendAuthorizedPost<T>(string url, object data, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(data)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            // ⚠️ Đọc cả response lỗi
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var result = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result;
            }
            catch
            {
                Console.WriteLine($"[BFF] BffService returned invalid JSON: {content}");
                return default;
            }
        }

        public async Task<List<LocationDto>> GetAvailableLocationsAsync()
        {
            var response = await _httpClient.GetAsync("/api/auth/locationsAvaible");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<List<LocationDto>>();
            return result ?? new();
        }



    }
}
