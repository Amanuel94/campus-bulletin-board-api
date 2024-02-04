// Purpose: This file contains the CreateUserDto class which implements the IUserDto interface. This class is used to create a new user in the database.
namespace Board.User.Service.DTOs;
public class CreateUserDto : IUserDto
{
    public string PasswordHash { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

}