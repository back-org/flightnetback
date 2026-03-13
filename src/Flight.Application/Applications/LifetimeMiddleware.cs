using Flight.Infrastructure.Contracts;
using Flight.Infrastructure.Implementations;
using Flight.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Flight.Application.Applications;

/// <summary>
/// Middleware d'extension pour l'enregistrement des services à durée de vie gérée (Scoped, Singleton).
/// Enregistre le <see cref="IRepositoryManager"/>, le <see cref="IServiceManager"/> et le logger.
/// </summary>
public static class LifetimeMiddleware
{
    /// <summary>
    /// Enregistre tous les services applicatifs principaux dans le conteneur DI.
    /// </summary>
    /// <param name="services">Le conteneur de services DI.</param>
    /// <remarks>
    /// Services enregistrés :
    /// <list type="bullet">
    ///   <item><description><see cref="ILoggerManager"/> (Scoped) — gestionnaire de logs NLog.</description></item>
    ///   <item><description><see cref="ILogger{T}"/> (Singleton) — logger générique ASP.NET Core.</description></item>
    ///   <item><description><see cref="IRepositoryManager"/> (Scoped) — accès aux dépôts de données.</description></item>
    ///   <item><description><see cref="IServiceManager"/> (Scoped) — couche service métier.</description></item>
    ///   <item><description><see cref="IMemoryCache"/> (Singleton) — cache mémoire in-process.</description></item>
    /// </list>
    /// </remarks>
    public static void AddRepoService(this IServiceCollection services)
    {
        services.AddScoped<ILoggerManager, LoggerManager>();
        services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IServiceManager, ServiceManager>();
        services.AddSingleton<IMemoryCache, MemoryCache>();
    }
}
