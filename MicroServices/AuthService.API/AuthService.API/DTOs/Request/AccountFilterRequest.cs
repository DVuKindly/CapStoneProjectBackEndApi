namespace AuthService.API.DTOs.Request
{
    public class AccountFilterRequest
    {
        public string[] RoleKeys { get; set; } = Array.Empty<string>();
        public Guid? LocationId { get; set; }
        public string? Search { get; set; } // tìm theo Email, UserName
        public bool? IsLocked { get; set; }
        public bool? EmailVerified { get; set; }

        public string? SortKey { get; set; } = "createdAt";
        public string? SortDirection { get; set; } = "desc";

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
