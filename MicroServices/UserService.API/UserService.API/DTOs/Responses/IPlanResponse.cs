namespace UserService.API.DTOs.Responses
{
    public interface IPlanResponse
    {
        Guid Id { get; }
        string? LocationName { get; }
    }

}
