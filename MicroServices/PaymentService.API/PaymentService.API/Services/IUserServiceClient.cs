
using PaymentService.API.DTOs.Requests;
using SharedKernel.DTOsChung;
using System.Threading.Tasks;

namespace PaymentService.API.Services.Interfaces
{
    public interface IUserServiceClient
    {
        Task<bool> NotifyPaymentSuccessAsync(Guid membershipRequestId);
        Task<bool> MarkMembershipRequestAsPaidAsync(MarkPaidRequestDto dto);
    }
}
