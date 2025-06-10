using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BffService.API.DTOs.Requests;
using BffService.API.DTOs.Responses;
using BffService.API.DTOs.User.Requests;
using BffService.API.DTOs.User.Responses;

namespace BffService.API.Services.User
{
    public interface IUserServiceClient
    {
        
        Task<UserProfileDto?> GetUserProfileAsync(Guid accountId);

      
        Task<BffBaseResponse> UpdateUserProfileAsync(Guid accountId, UpdateUserProfileDto dto);

       
        Task<BffBaseResponse> SubmitMembershipRequestAsync(Guid accountId, MembershipRequestDto dto);

       
        Task<List<PendingMembershipRequestDto>> GetPendingRequestsForStaffAsync(Guid staffAccountId);

       
        Task<BffBaseResponse> ApproveMembershipRequestAsync(ApproveMembershipRequestDto dto);

        
        Task<BffBaseResponse> RejectMembershipRequestAsync(RejectMembershipRequestDto dto);

       
        Task<PendingMembershipRequestDto?> GetRequestDetailAsync(Guid requestId);

        Task<List<LocationRegionDto>> GetAllLocationsAsync();
        Task<List<PendingMembershipRequestDto>> GetRequestHistoryAsync(Guid accountId);
    }
}
