namespace Board.User.Contracts.Contracts
{
    public class UserUpdated
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Department Department { get; set; }
        public int? Year { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string UserName { get; set; } = null!;

    }
}
