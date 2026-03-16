/*
 * Rôle métier du fichier: Composant applicatif.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Concrete' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Microsoft.Extensions.Configuration;

namespace Flight.Application.Concrete;

/// <summary>
/// Gestionnaire de configuration de l'application.
/// Charge les paramètres depuis <c>appsettings.json</c> au démarrage de l'application.
/// </summary>
/// <remarks>
/// Utilisé principalement pour lire la configuration JWT dans <c>Program.cs</c>
/// avant que le conteneur DI ne soit construit.
/// Pour accéder à la configuration dans les services, préférez l'injection de <see cref="IConfiguration"/>.
/// </remarks>
public class ConfigManager
{
    /// <summary>
    /// Obtient la configuration de l'application chargée depuis <c>appsettings.json</c>.
    /// </summary>
    public IConfiguration AppSetting { get; }

    /// <summary>
    /// Initialise une nouvelle instance du <see cref="ConfigManager"/>.
    /// Charge le fichier <c>appsettings.json</c> depuis le répertoire courant.
    /// </summary>
    public ConfigManager()
    {
        AppSetting = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
