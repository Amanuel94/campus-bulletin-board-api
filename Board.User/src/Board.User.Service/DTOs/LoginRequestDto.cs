namespace Board.User.Service.DTOs;

public class LoginRequestDto
{
    public string UserName { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}