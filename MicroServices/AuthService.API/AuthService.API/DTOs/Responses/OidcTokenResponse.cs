using System.Text.Json.Serialization;

namespace AuthService.API.DTOs.Responses
{
    public class OidcTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("id_token")]
        public string IdToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; } = 1800;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "Bearer";

        // 👇 THÊM CÁC TRƯỜNG DỮ LIỆU CẦN TRẢ
        [JsonPropertyName("user_id")]
        public Guid? UserId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("location_id")]
        public Guid? LocationId { get; set; }
    }



}
