namespace Board.User.Services.DTOs
{
    public record LoginResponseDto
    {
        public string Token { get; init; } = null!;
        public string RefreshToken { get; init; } = null!;
    }
}