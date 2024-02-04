using System.Text;
using Board.Auth.Jwt.Interfaces;
using Board.Auth.Service.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace Board.Common.Settings;

/// <summary>
/// Provides extension methods for configuring JWT authentication in the service collection.
/// </summary>
public static class Extensions
{
    private static JWTSettings? _jwtSettings;

    /// <summary>
    /// Adds JWT authentication and configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the authentication to.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddIdentityAuth(this IServiceCollection services)
    {
        // Retrieve the configuration from the service provider
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        // Configure JWTSettings using the provided configuration
        services.Configure<JWTSettings>(configuration!.GetSection(nameof(JWTSettings)));

        // Add JwtService and IJwtService to the service collection
        services.AddScoped<IJwtService, JwtService>();

        // Add JWT authentication with the specified options
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Retrieve JWTSettings from the configuration
                _jwtSettings = configuration!.GetSection(nameof(JWTSettings)).Get<JWTSettings>()!;

                // Configure token validation parameters
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                    ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }
}