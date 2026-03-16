/*
 * Rôle métier du fichier: Fournir les implémentations techniques des services et dépôts métier.
 * Description: Ce fichier participe au sous-domaine 'Flight.Infrastructure/Implementations' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using NLog;

namespace Flight.Infrastructure.Implementations;

/// <summary>
/// Implémentation du gestionnaire de logs utilisant NLog.
/// Wrapping de <see cref="NLog.ILogger"/> pour respecter l'interface <see cref="ILoggerManager"/>.
/// </summary>
public class LoggerManager : ILoggerManager
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Enregistre un message de niveau DEBUG. Utilisé pour les informations de débogage détaillées.
    /// </summary>
    /// <param name="message">Le message à enregistrer.</param>
    public void LogDebug(string message) => Logger.Debug(message);

    /// <summary>
    /// Enregistre un message de niveau ERROR. Utilisé pour les erreurs applicatives non fatales.
    /// </summary>
    /// <param name="message">Le message d'erreur à enregistrer.</param>
    public void LogError(string message) => Logger.Error(message);

    /// <summary>
    /// Enregistre un message de niveau INFO. Utilisé pour les événements applicatifs normaux.
    /// </summary>
    /// <param name="message">Le message informatif à enregistrer.</param>
    public void LogInfo(string message) => Logger.Info(message);

    /// <summary>
    /// Enregistre un message de niveau WARNING. Utilisé pour les situations anormales non critiques.
    /// </summary>
    /// <param name="message">Le message d'avertissement à enregistrer.</param>
    public void LogWarn(string message) => Logger.Warn(message);
}
