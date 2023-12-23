namespace Board.User.Services.DTOs
{
    public class LoginResponseDto : IUserDto
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}