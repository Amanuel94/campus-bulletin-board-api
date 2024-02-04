// Purpose: This file contains the DTO for the login request.

namespace Board.User.Service.DTOs;

public class LoginRequestDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}