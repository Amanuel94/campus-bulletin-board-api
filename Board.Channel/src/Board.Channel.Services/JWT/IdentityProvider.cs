// Purpose: Provides functionality to retrieve the user identity from the HTTP context using JWT authentication.
using System.IdentityModel.Tokens.Jwt;
using Board.Channel.Service.Jwt.Interfaces;
using Microsoft.Extensions.Primitives;

namespace Board.Channel.Service.Jwt;

/// <summary>
/// Provides functionality to retrieve the user identity from the HTTP context using JWT authentication.
/// </summary>
public class IdentityProvider
{
    private readonly HttpContext _httpContext;
    private readonly IJwtService _jwtService;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityProvider"/> class.
    /// </summary>
    /// <param name="httpContext">The HTTP context.</param>
    /// <param name="jwtService">The JWT service.</param>
    public IdentityProvider(HttpContext httpContext, IJwtService jwtService)
    {
        _httpContext = httpContext;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Gets the user ID from the JWT token in the Authorization header of the HTTP request.
    /// </summary>
    /// <returns>The user ID as a <see cref="Guid"/> if the token is valid; otherwise, returns <see cref="Guid.Empty"/>.</returns>
    public Guid GetUserId()
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