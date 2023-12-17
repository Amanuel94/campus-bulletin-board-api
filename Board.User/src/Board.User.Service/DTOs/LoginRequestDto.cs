namespace Board.User.Services.DTOs;

public record LoginRequestDto
{
    public string UserName { get; init; } = null!;
    public string Password { get; init; } = null!;
}