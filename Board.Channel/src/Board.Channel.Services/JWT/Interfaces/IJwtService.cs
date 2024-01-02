namespace Board.Channel.Service.Jwt.Interfaces
{
    public interface IJwtService
    {
        bool IsTokenValid(string token);
    }
}