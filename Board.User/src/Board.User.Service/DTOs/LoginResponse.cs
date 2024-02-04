// Purpose: This file contains the DTO for the login response.
namespace Board.User.Service.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}