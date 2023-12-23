namespace Board.User.Services.DTOs;

public class GeneralUserDto : IUserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Department { get; set; } = null!;
    public byte[]? Avatar { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}