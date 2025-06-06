using UserService.API.Entities;

namespace UserService.API.Repositories.Implementations
{
    public interface ICoachProfileRepository
    {
        Task<UserProfile?> GetUserByAccountIdAsync(Guid accountId);
        Task AddCoachProfileAsync(CoachProfile profile);
    }
}
