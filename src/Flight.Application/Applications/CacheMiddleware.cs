using Microsoft.Extensions.DependencyInjection;

namespace Flight.Application.Applications;

/// <summary>
/// Middleware d'extension pour la configuration du cache de réponse HTTP.
/// Utilise le cache mémoire intégré d'ASP.NET Core pour améliorer les performances
/// sur les endpoints en lecture.
/// </summary>
public static class CacheMiddleware
{
    /// <summary>
    /// Enregistre les services de mise en cache des réponses HTTP dans le conteneur DI.
    /// Active à la fois le <c>ResponseCaching</c> (middleware HTTP) et le <c>MemoryCache</c>
    /// (cache en mémoire du processus).
    /// </summary>
    /// <param name="services">Le conteneur de services DI.</param>
    /// <remarks>
    /// Pour activer le cache sur un endpoint, décorez le contrôleur ou l'action avec :
    /// <code>[ResponseCache(Duration = 60)]</code>
    /// Et ajoutez <c>app.UseResponseCaching()</c> dans le pipeline dans <c>Program.cs</c>.
    /// </remarks>
    public static void AddResponseCache(this IServiceCollection services)
    {
        services.AddResponseCaching();
        services.AddMemoryCache();
    }
}
