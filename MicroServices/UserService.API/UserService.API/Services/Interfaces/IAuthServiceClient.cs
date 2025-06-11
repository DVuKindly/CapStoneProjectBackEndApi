namespace UserService.API.Services.Interfaces
{
    public interface IAuthServiceClient
    {
        Task<bool> PromoteUserToMemberAsync(Guid accountId);
    }

}
