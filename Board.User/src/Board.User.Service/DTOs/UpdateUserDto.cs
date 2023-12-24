namespace Board.User.Service.DTOs;

public class UpdateUserDto : IUserDto
{
    public Guid Id { get; set; }
    public DateTime ModifiedDate { get; set; }

}