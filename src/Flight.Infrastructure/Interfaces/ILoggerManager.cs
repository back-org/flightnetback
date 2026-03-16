/*
 * Rôle métier du fichier: Définir les contrats métier et techniques entre couches.
 * Description: Ce fichier participe au sous-domaine 'Flight.Infrastructure/Interfaces' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

namespace Flight.Infrastructure.Interfaces;

/// <summary>
/// Interface du gestionnaire de logs de l'application.
/// Abstraction permettant de découpler les couches métier du framework de logging (NLog, Serilog, etc.).
/// </summary>
public interface ILoggerManager
{
    /// <summary>Enregistre un message de niveau INFO.</summary>
    /// <param name="message">Le message informatif.</param>
    void LogInfo(string message);

    /// <summary>Enregistre un message de niveau WARNING.</summary>
    /// <param name="message">Le message d'avertissement.</param>
    void LogWarn(string message);

    /// <summary>Enregistre un message de niveau DEBUG.</summary>
    /// <param name="message">Le message de débogage.</param>
    void LogDebug(string message);

    /// <summary>Enregistre un message de niveau ERROR.</summary>
    /// <param name="message">Le message d'erreur.</param>
    void LogError(string message);
}
