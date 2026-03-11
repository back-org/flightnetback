using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Flight.Infrastructure.Auth;

/// <summary>
/// Service hébergé ASP.NET Core chargé de nettoyer périodiquement les refresh tokens expirés.
/// S'exécute en arrière-plan toutes les minutes pour libérer la mémoire des tokens obsolètes.
/// </summary>
public class JwtRefreshTokenCache(IJwtAuthManager jwtAuthManager) : IHostedService, IDisposable
{
    private Timer _timer = null!;

    /// <summary>
    /// Démarre le timer de nettoyage des tokens expirés.
    /// Le nettoyage s'effectue immédiatement au démarrage puis toutes les minutes.
    /// </summary>
    /// <param name="stoppingToken">Token d'annulation signalant l'arrêt du service.</param>
    /// <returns>Une tâche complétée représentant le démarrage synchrone du service.</returns>
    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(DoWork!, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    /// <summary>
    /// Callback du timer : délègue la suppression des tokens expirés au <see cref="IJwtAuthManager"/>.
    /// </summary>
    /// <param name="state">Paramètre d'état du timer (non utilisé).</param>
    private void DoWork(object state)
    {
        jwtAuthManager.RemoveExpiredRefreshTokens(DateTime.Now);
    }

    /// <summary>
    /// Arrête le timer de nettoyage en désactivant ses déclenchements périodiques.
    /// </summary>
    /// <param name="stoppingToken">Token d'annulation signalant l'arrêt demandé.</param>
    /// <returns>Une tâche complétée représentant l'arrêt synchrone du service.</returns>
    public Task StopAsync(CancellationToken stoppingToken)
    {
        _timer.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Libère les ressources du timer natif.
    /// </summary>
    public void Dispose()
    {
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}
