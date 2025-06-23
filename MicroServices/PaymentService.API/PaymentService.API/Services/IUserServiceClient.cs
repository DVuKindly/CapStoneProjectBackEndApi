using PaymentService.API.DTOs.Requests;

using System.Threading.Tasks;

namespace PaymentService.API.Services.Interfaces
{
    public interface IUserServiceClient
    {
        Task<bool> MarkMembershipRequestAsPaidAsync(MarkPaidRequestDto dto);
    }
}
