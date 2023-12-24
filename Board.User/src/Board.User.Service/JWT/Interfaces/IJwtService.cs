namespace Board.User.Service.Jwt.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Models.User user);
        bool IsTokenValid(string token);
    }
}