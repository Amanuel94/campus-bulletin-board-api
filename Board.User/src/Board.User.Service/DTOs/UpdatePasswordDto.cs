namespace Board.User.Services.DTOs
{
    public record UpdatePasswordDto
    {
        public Guid Id { get; init; }
        public string CurrentPassword { get; init; } = null!;
        public string NewPassword { get; init; } = null!;
    }
}