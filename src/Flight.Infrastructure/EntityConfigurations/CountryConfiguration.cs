/*
 * Rôle métier du fichier: Gérer la persistance et la cartographie des objets métier.
 * Description: Ce fichier participe au sous-domaine 'Flight.Infrastructure/EntityConfigurations' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flight.Infrastructure.EntityConfigurations;

/// <summary>
/// Configuration Fluent API d'Entity Framework Core pour l'entité <see cref="Country"/>.
/// Définit les relations, navigations et comportements spécifiques à la table Countries.
/// </summary>
public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    /// <summary>
    /// Configure le modèle de l'entité <see cref="Country"/>.
    /// Active le chargement automatique (AutoInclude) de la navigation <see cref="Country.Cities"/>
    /// afin que les villes soient toujours incluses dans les requêtes sur les pays.
    /// </summary>
    /// <param name="builder">Le constructeur de configuration de l'entité.</param>
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Navigation(e => e.Cities).AutoInclude();
    }
}
