// Purpose: Interface for the JWT service.
namespace Board.Channel.Service.Jwt.Interfaces
{
    /// <summary>
    /// Represents a service for handling JSON Web Tokens (JWT).
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Checks if the given JWT token is valid.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>True if the token is valid, otherwise false.</returns>
        bool IsTokenValid(string token);
    }
}