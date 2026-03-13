using System.Text;
using Flight.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Flight.Application.Applications;

/// <summary>
/// Middleware d'extension pour la configuration du service d'authentification JWT.
/// Configure le pipeline d'authentification Bearer avec validation complète des tokens.
/// </summary>
public static class JwtMiddleware
{
    /// <summary>
    /// Enregistre et configure le service d'authentification JWT dans le conteneur DI.
    /// </summary>
    /// <param name="services">Le conteneur de services DI.</param>
    /// <param name="jwtTokenConfig">
    /// La configuration JWT (secret, issuer, audience, durées d'expiration).
    /// Doit être chargée depuis <c>appsettings.json</c> via la clé <c>jwtTokenConfig</c>.
    /// </param>
    /// <remarks>
    /// Ce middleware enregistre automatiquement :
    /// <list type="bullet">
    ///   <item><description><see cref="JwtTokenConfig"/> comme singleton de configuration.</description></item>
    ///   <item><description><see cref="IJwtAuthManager"/> / <see cref="JwtAuthManager"/> pour la génération et validation des tokens.</description></item>
    ///   <item><description><see cref="JwtRefreshTokenCache"/> comme service hébergé pour nettoyer les tokens expirés.</description></item>
    ///   <item><description><see cref="IUserService"/> / <see cref="UserService"/> pour la validation des identifiants.</description></item>
    /// </list>
    /// </remarks>
    public static void AddJwtService(this IServiceCollection services, JwtTokenConfig jwtTokenConfig)
    {
        services.AddSingleton(jwtTokenConfig);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtTokenConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                ValidAudience = jwtTokenConfig.Audience,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });

        services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
        services.AddHostedService<JwtRefreshTokenCache>();
        services.AddScoped<IUserService, UserService>();
    }
}
