using Flight.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flight.Application.Applications;

/// <summary>
/// Middleware d'extension pour la configuration de la base de données.
/// Enregistre le contexte Entity Framework <see cref="FlightContext"/> dans le conteneur DI.
/// </summary>
public static class DatabaseMiddleware
{
    /// <summary>
    /// Configure et enregistre le <see cref="FlightContext"/> en utilisant la chaîne de connexion
    /// spécifiée dans la configuration de l'application (clé <c>ConnectionStrings:DbConn</c>).
    /// </summary>
    /// <param name="services">Le conteneur de services DI.</param>
    /// <param name="configuration">La configuration de l'application (appsettings.json, variables d'environnement, etc.).</param>
    /// <exception cref="InvalidOperationException">
    /// Levée si la chaîne de connexion <c>DbConn</c> est absente ou vide.
    /// </exception>
    public static void AddDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("DbConn")
            ?? throw new InvalidOperationException(
                "La chaîne de connexion 'DbConn' est manquante dans la configuration.");

        services.AddDbContext<FlightContext>(opt =>
            opt.UseMySql(connString, ServerVersion.AutoDetect(connString)));
    }

    /// <summary>
    /// Configure et enregistre le <see cref="FlightContext"/> en lisant la chaîne de connexion
    /// depuis la variable d'environnement <c>DB_CONNECTION_STRING</c>.
    /// Si la variable n'est pas définie, une exception est levée.
    /// </summary>
    /// <param name="services">Le conteneur de services DI.</param>
    /// <exception cref="InvalidOperationException">
    /// Levée si la variable d'environnement <c>DB_CONNECTION_STRING</c> n'est pas définie.
    /// </exception>
    public static void AddDataContext(this IServiceCollection services)
    {
        var connString = System.Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? throw new InvalidOperationException(
                "La variable d'environnement 'DB_CONNECTION_STRING' n'est pas définie. " +
                "Exemple : server=127.0.0.1;port=3306;user=root;password=secret;database=flights");

        services.AddDbContext<FlightContext>(opt =>
            opt.UseMySql(connString, ServerVersion.AutoDetect(connString)));
    }
}
