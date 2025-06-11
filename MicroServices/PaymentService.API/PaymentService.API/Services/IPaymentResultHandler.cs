namespace PaymentService.API.Services
{
    public interface IPaymentResultHandler
    {
        Task HandleSuccessfulPaymentAsync(Guid membershipRequestId);
    }

}
