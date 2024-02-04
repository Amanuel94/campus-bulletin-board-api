// Purpose: This file contains the GeneralUserDto class which implements the IUserDto interface. This class is used to represent a user in the system.
namespace Board.User.Service.DTOs;

public class GeneralUserDto : IUserDto
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}