using Board.User.Services.Models;

namespace Board.User.Services.DTOs
{
    public abstract class IUserDto
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public Department? Department { get; init; }
        public int? Year { get; init; }
        public string? PhoneNumber { get; init; }
        public string? UserName { get; init; }
        public byte[]? Avatar { get; init; }
    }
}