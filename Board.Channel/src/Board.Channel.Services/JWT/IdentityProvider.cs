using System.IdentityModel.Tokens.Jwt;
using Board.Channel.Service.Jwt.Interfaces;
using Microsoft.Extensions.Primitives;

namespace Board.Channel.Service.Jwt;

public class IdentityProvider
{
    private readonly HttpContext _httpContext;
    private readonly IJwtService _jwtService;
    public IdentityProvider(HttpContext httpContext, IJwtService jwtService)
    {
        _httpContext = httpContext;
        _jwtService = jwtService;
    }

    public  Guid GetUserId()
    {
        if (_httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
        {
            string bearerToken = authHeader.ToString().Replace("Bearer ", "");
            if (_jwtService.IsTokenValid(bearerToken) == false)
            {
                return Guid.Empty;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt = tokenHandler.ReadJwtToken(bearerToken);
            var userId = Guid.Parse(jwt.Claims.First(x => x.Type == "nameid").Value);
            return userId;
        }

        else
        {
            return Guid.Empty;
        }
    }
}