namespace Board.Auth.Jwt.Interfaces
{
    public interface IJwtService
    {
        bool IsTokenValid(string token);
    }
}