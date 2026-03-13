using Flight.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flight.Application.Applications;

/// <summary>
/// Middleware d'extension permettant d'enregistrer le contexte
/// Entity Framework dans le conteneur d'injection de dépendances.
/// </summary>
public static class DatabaseMiddleware
{
    /// <summary>
    /// Configure et enregistre le <see cref="FlightContext"/> en utilisant
    /// la chaîne de connexion provenant soit :
    /// 
    /// 1. du fichier .env (DB_CONNECTION_STRING)
    /// 2. de la configuration appsettings.json
    /// 3. des variables d'environnement système
    /// </summary>
    /// <param name="services">Conteneur DI.</param>
    /// <param name="configuration">Configuration ASP.NET.</param>
    /// <exception cref="InvalidOperationException">
    /// Levée si aucune chaîne de connexion n'est trouvée.
    /// </exception>
    public static void AddDataContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1️⃣ Priorité au fichier .env
        var envConn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        // 2️⃣ fallback appsettings.json
        var configConn = configuration.GetConnectionString("DbConn");

        var connString = envConn ?? configConn;

        if (string.IsNullOrWhiteSpace(connString))
        {
            throw new InvalidOperationException(
                """
                Aucune chaîne de connexion trouvée.

                Vérifiez :
                - le fichier .env (DB_CONNECTION_STRING)
                - appsettings.json (ConnectionStrings:DbConn)

                Exemple .env :
                DB_CONNECTION_STRING=server=127.0.0.1;port=3306;user=root;password=secret;database=flights
                """
            );
        }

        services.AddDbContext<FlightContext>(options =>
            options.UseMySql(
                connString,
                ServerVersion.AutoDetect(connString)
            )
        );
    }
}