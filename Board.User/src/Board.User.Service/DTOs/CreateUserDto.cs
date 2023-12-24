namespace Board.User.Service.DTOs;
public class CreateUserDto : IUserDto
{
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

}