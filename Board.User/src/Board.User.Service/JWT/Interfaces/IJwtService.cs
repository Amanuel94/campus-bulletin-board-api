namespace Board.User.Services.Jwt.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Models.User user);
        bool IsTokenValid(string token);
    }
}