using Board.Common.Models;

namespace Board.Channel.Service.Model;

public class UserItem :BaseEntity{

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required Department Department { get; set; }
    public int? Year { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string UserName { get; set; } = null!;

}