using System.Threading.Tasks;
using SharedKernel.DTOsChung;

namespace PaymentService.API.Services
{
    public interface IPaymentResultHandler
    {
        Task HandleSuccessfulPaymentAsync(MarkPaidRequestDto dto);
    }
}
