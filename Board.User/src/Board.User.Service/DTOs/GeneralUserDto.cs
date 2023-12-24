namespace Board.User.Service.DTOs;

public class GeneralUserDto : IUserDto
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}