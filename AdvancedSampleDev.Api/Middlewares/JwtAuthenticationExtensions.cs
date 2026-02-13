using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AdvancedSampleDev.Api.Middlewares;

/// <summary>
/// Extension pour configurer l'authentification JWT
/// </summary>
public static class JwtAuthenticationExtensions
{
    /// <summary>
    /// Configure l'authentification et l'autorisation JWT
    /// </summary>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Récupération de la clé secrète depuis les variables d'environnement
        var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
            ?? throw new InvalidOperationException("JWT_SECRET_KEY non définie dans les variables d'environnement");
        
        var jwtIssuer = configuration["Jwt:Issuer"] 
            ?? throw new InvalidOperationException("JWT Issuer non configuré");
        
        var jwtAudience = configuration["Jwt:Audience"] 
            ?? throw new InvalidOperationException("JWT Audience non configurée");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
                ClockSkew = TimeSpan.Zero // Pas de délai de grâce pour l'expiration
            };
        });

        services.AddAuthorization();

        return services;
    }
}
