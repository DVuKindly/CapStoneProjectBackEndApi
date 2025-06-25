namespace UserService.API.DTOs.Requests
{
    public class UpdateUserProfileStatusDto
    {
        public string OnboardingStatus { get; set; } = string.Empty;
        // các trường trạng thái khác nếu cần
    }

}
