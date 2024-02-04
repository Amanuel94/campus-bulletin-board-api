//Purspose: This file contains the User model which is used to represent the user entity in the database.
using Board.Common.Models;

namespace Board.User.Service.Models;
public class User:BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required Department Department { get; set; }
    public int? Year { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public byte[]? Avatar { get; set; }
    public string PasswordHash { get; set; } = null!;
};

