using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Flight.Infrastructure.Database;

namespace Flight.Application.Applications;

/// <summary>
/// Middleware d'extension pour la configuration d'ASP.NET Core Identity.
/// Utilisé si l'on souhaite activer l'authentification basée sur Identity (cookies + comptes ASP.NET).
/// </summary>
/// <remarks>
/// <b>Note :</b> Ce middleware est optionnel dans cette architecture.
/// L'authentification principale est gérée via JWT dans <see cref="JwtMiddleware"/>.
/// Activez Identity uniquement si vous avez besoin de gestion de comptes utilisateurs via la base de données.
/// </remarks>
public static class AuthMiddleware
{
    /// <summary>
    /// Configure ASP.NET Core Identity avec les règles de mot de passe, verrouillage et utilisateur.
    /// Nécessite que <see cref="FlightContext"/> soit déjà enregistré dans le conteneur DI.
    /// </summary>
    /// <param name="services">Le conteneur de services DI.</param>
    public static void AddIdentityService(this IServiceCollection services)
    {
        services.AddDefaultIdentity<IdentityUser>(options =>
            options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<FlightContext>();

        services.Configure<IdentityOptions>(options =>
        {
            // Règles de mot de passe
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            // Règles de verrouillage de compte
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // Règles sur le nom d'utilisateur
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });
    }

    /// <summary>
    /// Configure le cookie d'authentification Identity (session, chemins de redirection, expiration).
    /// </summary>
    /// <param name="services">Le conteneur de services DI.</param>
    public static void AddIdentityCookie(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });
    }
}
