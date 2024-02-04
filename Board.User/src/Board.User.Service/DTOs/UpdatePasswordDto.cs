// Purpose: This file contains the UpdatePasswordDto class.
namespace Board.User.Service.DTOs
{
    public class UpdatePasswordDto : IUserDto
    {
        public string CurrentPassword { get; init; } = null!;
        public string NewPassword { get; init; } = null!;
    }
}