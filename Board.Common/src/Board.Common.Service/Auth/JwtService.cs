using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Board.Auth.Jwt.Interfaces;
using Board.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Board.Auth.Service.Jwt;

/// <summary>
/// Represents a service for handling JSON Web Tokens (JWT).
/// </summary>
public class JwtService : IJwtService
{
    private readonly JWTSettings _jwtSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtService"/> class.
    /// </summary>
    /// <param name="jwtSettings">The JWT settings.</param>
    public JwtService(IOptions<JWTSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    /// <summary>
    /// Determines whether the specified token is valid.
    /// </summary>
    /// <param name="token">The token to validate.</param>
    /// <returns><c>true</c> if the token is valid; otherwise, <c>false</c>.</returns>
    public bool IsTokenValid(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Issuer,
                ValidateLifetime = true
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }
}
