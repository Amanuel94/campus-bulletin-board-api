namespace Board.User.Service.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}