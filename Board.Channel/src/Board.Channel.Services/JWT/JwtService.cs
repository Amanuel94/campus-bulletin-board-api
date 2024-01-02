using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Board.Channel.Service.Jwt.Interfaces;
using Board.User.Service.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Board.User.Service.Jwt;

public class JwtService : IJwtService
{
    private readonly JWTSettings _jwtSettings;

    public JwtService(IOptions<JWTSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
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
