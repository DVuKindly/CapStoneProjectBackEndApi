
using PaymentService.API.DTOs.Requests;
using SharedKernel.DTOsChung;
using System.Threading.Tasks;

namespace PaymentService.API.Services.Interfaces
{
    public interface IUserServiceClient
    {
        Task<bool> NotifyPaymentSuccessAsync(Guid paymentRequestId, Guid? membershipRequestId = null, bool? isDirectMembership = null);
        Task<bool> MarkMembershipRequestAsPaidAsync(MarkPaidRequestDto dto);
    }


}
