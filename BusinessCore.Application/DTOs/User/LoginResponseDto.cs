namespace BusinessCore.Application.DTOs.User
{
    public class LoginResponseDto
    {
        public UserResponseDto User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}