using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

using BffService.API.DTOs.Requests;
using BffService.API.DTOs.Responses;
using BffService.API.DTOs.User.Responses;
using BffService.API.DTOs.User.Requests; // chứa BffBaseResponse

namespace BffService.API.Services.User
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserServiceClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserServiceClient(HttpClient httpClient, ILogger<UserServiceClient> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
         
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(Guid accountId)
        {
            AttachAuthorizationHeader();
            return await GetJson<UserProfileDto>("/api/userprofiles/profileme");
        }

        public async Task<BffBaseResponse> SubmitMembershipRequestAsync(Guid accountId, MembershipRequestDto dto)
        {
            AttachAuthorizationHeader();
            return await PostJson<BffBaseResponse>("/api/membership/requestMember", dto);
        }

        public async Task<List<PendingMembershipRequestDto>> GetPendingRequestsForStaffAsync(Guid staffAccountId)
        {
            AttachAuthorizationHeader();
            return await GetJson<List<PendingMembershipRequestDto>>("/api/membership/pending-requests");
        }

        public async Task<BffBaseResponse> ApproveMembershipRequestAsync(ApproveMembershipRequestDto dto)
        {
            AttachAuthorizationHeader();
            return await PostJson<BffBaseResponse>("/api/membership/approveRequest", dto);
        }

        public async Task<BffBaseResponse> RejectMembershipRequestAsync(RejectMembershipRequestDto dto)
        {
            AttachAuthorizationHeader();
            return await PostJson<BffBaseResponse>("/api/membership/rejectRequest", dto);
        }

        public async Task<PendingMembershipRequestDto?> GetRequestDetailAsync(Guid requestId)
        {
            AttachAuthorizationHeader();
            return await GetJson<PendingMembershipRequestDto>($"/api/membership/request-detail/{requestId}");
        }

        public async Task<List<PendingMembershipRequestDto>> GetRequestHistoryAsync(Guid accountId)
        {
            AttachAuthorizationHeader();
            return await GetJson<List<PendingMembershipRequestDto>>("/api/membership/history");
        }

        public async Task<BffBaseResponse> UpdateUserProfileAsync(Guid accountId, UpdateUserProfileDto dto)
        {
            AttachAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync("/api/userprofiles/updateprofile", dto);
            return await response.Content.ReadFromJsonAsync<BffBaseResponse>()
                   ?? new BffBaseResponse { Success = false, Message = "Lỗi khi cập nhật." };
        }

        // ======= PRIVATE HTTP HELPERS =======

        private async Task<T?> GetJson<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private async Task<T?> PostJson<T>(string url, object body)
        {
            var response = await _httpClient.PostAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>();
        }

        private async Task<T?> PutJson<T>(string url, object body)
        {
            var response = await _httpClient.PutAsJsonAsync(url, body);
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>();
        }
        private void AttachAuthorizationHeader()
        {
            var token = _httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(token))
            {
                // Remove old header (if any)
                _httpClient.DefaultRequestHeaders.Authorization = null;

                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Replace("Bearer ", ""));
            }
        }
        public async Task<List<LocationRegionDto>> GetAllLocationsAsync()
        {
            AttachAuthorizationHeader(); // nếu location yêu cầu Auth
            return await GetJson<List<LocationRegionDto>>("/api/locations")
                   ?? new List<LocationRegionDto>();
        }


    }
}
