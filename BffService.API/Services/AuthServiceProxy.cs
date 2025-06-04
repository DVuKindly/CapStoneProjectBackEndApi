using System.Net.Http.Json;
using System.Net.Http.Headers;
using BffService.API.DTOs.Auth.Request;
using BffService.API.DTOs.Auth.Response;
using System.Text.Json.Serialization;
using System.Text;

namespace BffService.API.Services
{
    public class AuthProxyService
    {
        private readonly HttpClient _httpClient;

        public AuthProxyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/refresh", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<BaseResponse?> VerifyEmailAsync(VerifyEmailRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/verify-email", request);
            return await response.Content.ReadFromJsonAsync<BaseResponse>();
        }

        public async Task<BaseResponse?> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/forgot-password", request);
            return await response.Content.ReadFromJsonAsync<BaseResponse>();
        }

        public async Task<BaseResponse?> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/reset-password", request);
            return await response.Content.ReadFromJsonAsync<BaseResponse>();
        }

        public async Task<AuthResponse?> GoogleLoginAsync(GoogleLoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/google-login", request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new AuthResponse
                {
                    Success = false,
                    Message = $"AuthService error: {response.StatusCode} - {errorContent}"
                };
            }

            return await response.Content.ReadFromJsonAsync<AuthResponse>();
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
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/logout");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);  

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new AuthResponse
                    {
                        Success = false,
                        Message = $"AuthService error: {response.StatusCode} - {errorContent}"
                    };
                }
                return await response.Content.ReadFromJsonAsync<AuthResponse>();
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Exception occurred: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponse?> ChangePasswordAsync(ChangePasswordRequest request, string token)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/auth/change-password");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Dùng JsonContent.Create thay vì StringContent
                requestMessage.Content = JsonContent.Create(request);

                var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new AuthResponse
                    {
                        Success = false,
                        Message = $"AuthService error: {response.StatusCode} - {errorContent}"
                    };
                }

                return await response.Content.ReadFromJsonAsync<AuthResponse>();
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Exception occurred: {ex.Message}"
                };
            }
        }




    }
}
