/*
 * Rôle métier du fichier: Orchestrer le pipeline d’exécution transverse (middleware, bootstrap, configuration).
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Applications' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Microsoft.Extensions.DependencyInjection;

namespace Flight.Application.Applications;

/// <summary>
/// Middleware d'extension pour la configuration des politiques CORS (Cross-Origin Resource Sharing).
/// Permet aux applications front-end Angular d'accéder à l'API depuis un domaine différent.
/// </summary>
public static class CorsMiddleware
{
    /// <summary>
    /// Enregistre une politique CORS par défaut dans le conteneur DI, autorisant toutes les origines,
    /// tous les en-têtes et toutes les méthodes HTTP.
    /// </summary>
    /// <param name="services">Le conteneur de services DI.</param>
    /// <remarks>
    /// <b>Attention :</b> Cette configuration est permissive et adaptée au développement.
    /// En production, restreignez les origines autorisées :
    /// <code>
    /// policy.WithOrigins("https://votreapp.com").AllowAnyHeader().AllowAnyMethod();
    /// </code>
    /// </remarks>
    public static void ConfigureCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod());
        });
    }
}
