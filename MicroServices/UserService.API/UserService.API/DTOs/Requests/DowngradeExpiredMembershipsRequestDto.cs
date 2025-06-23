namespace UserService.API.DTOs.Requests
{
    public class DowngradeExpiredMembershipsRequestDto
    {
        public bool Force { get; set; } = false;
    }

}
