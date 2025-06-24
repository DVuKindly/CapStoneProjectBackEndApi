namespace PaymentService.API.Services
{
    public interface IHttpContextHelper
    {
        string GetClientIpAddress();
    }
}
