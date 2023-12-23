namespace Board.User.Services.DTOs;

public class LoginRequestDto : IUserDto
{
    public string UserName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}