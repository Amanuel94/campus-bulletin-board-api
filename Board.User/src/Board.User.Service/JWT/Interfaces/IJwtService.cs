
namespace Board.User.Service.Jwt.Interfaces
{
    /// <summary>
    /// Represents a service for handling JSON Web Tokens (JWT).
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <returns>The generated JWT token.</returns>
        string GenerateToken(Models.User user);

        /// <summary>
        /// Checks if the specified JWT token is valid.
        /// </summary>
        /// <param name="token">The JWT token to validate.</param>
        /// <returns>True if the token is valid; otherwise, false.</returns>
        bool IsTokenValid(string token);
    }
}