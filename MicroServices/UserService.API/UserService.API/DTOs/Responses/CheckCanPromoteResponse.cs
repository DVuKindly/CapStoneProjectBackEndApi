namespace UserService.API.DTOs.Responses
{
    public class CheckCanPromoteResponse
    {
        public bool CanPromote { get; set; }
        public string? Reason { get; set; } 
        
    }
}
