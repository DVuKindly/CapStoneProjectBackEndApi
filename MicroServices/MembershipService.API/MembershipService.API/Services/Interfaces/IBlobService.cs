namespace MembershipService.API.Services.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadAsync(IFormFile file);
    }

}