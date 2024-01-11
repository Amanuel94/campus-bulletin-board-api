using System.Text;
using Board.Auth.Jwt.Interfaces;
using Board.Auth.Service.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace Board.Common.Settings;

public static class Extensions
{
    private static JWTSettings? _jwtSettings;

    public static IServiceCollection AddIdentityAuth(this IServiceCollection services)
    {

        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        services.Configure<JWTSettings>(configuration!.GetSection(nameof(JWTSettings)));

        services.AddScoped<IJwtService, JwtService>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {

                _jwtSettings = configuration!.GetSection(nameof(JWTSettings)).Get<JWTSettings>()!;
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