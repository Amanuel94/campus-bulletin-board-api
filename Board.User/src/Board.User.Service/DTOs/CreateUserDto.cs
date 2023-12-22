using Board.User.Services.Models;

namespace Board.User.Services.DTOs;
public record CreateUserDto
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public Department Department { get; init; }
    public int? Year { get; init; }
    public string PhoneNumber { get; init; } = null!;
    public string UserName { get; init; } = null!;
    public byte[]? Avatar { get; init; }
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

}