using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Board.User.Services.Jwt.Interfaces;
using Board.User.Services.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Board.User.Services.Jwt;

public class JwtService : IJwtService
{
    private readonly JWTSettings _jwtSettings;

    public JwtService(IOptions<JWTSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(Models.User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),}),

            Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
