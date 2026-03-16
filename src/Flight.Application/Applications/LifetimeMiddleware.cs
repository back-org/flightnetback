using Flight.Infrastructure.AuditTrail;
using Flight.Infrastructure.Contracts;
using Flight.Infrastructure.Implementations;
using Flight.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Flight.Application.Applications;

/// <summary>
/// Middleware d'extension pour l'enregistrement des services applicatifs
/// dans le conteneur d'injection de dépendances.
/// </summary>
public static class LifetimeMiddleware
{
    /// <summary>
    /// Enregistre les services principaux de l'application.
    /// </summary>
    /// <param name="services">Le conteneur de services.</param>
    public static void AddRepoService(this IServiceCollection services)
    {
        // Permet d'accéder au HttpContext courant depuis les services.
        services.AddHttpContextAccessor();

        // Services de logging.
        services.AddScoped<ILoggerManager, LoggerManager>();        

        // Repositories et services métier.
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IServiceManager, ServiceManager>();

        // Cache mémoire local.
        services.AddSingleton<IMemoryCache, MemoryCache>();

        // Service d'audit.
        services.AddScoped<IAuditTrailService, AuditTrailService>();
    }
}
