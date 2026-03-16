/*
 * Rôle métier du fichier: Modéliser les entités métier du domaine aérien.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain/Entities' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un avion utilisé pour effectuer les vols.
/// </summary>
[Table("Aircrafts")]
public class Aircraft
{
    /// <summary>
    /// Identifiant unique de l'avion.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Numéro d'immatriculation de l'avion.
    /// Exemple : 5R-ABC
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string RegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Fabricant de l'avion.
    /// Exemple : Airbus, Boeing.
    /// </summary>
    [MaxLength(50)]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Modèle de l'avion.
    /// Exemple : A320, B737.
    /// </summary>
    [MaxLength(50)]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de sièges en classe affaires.
    /// </summary>
    public int BusinessCapacity { get; set; }

    /// <summary>
    /// Nombre de sièges en classe économique.
    /// </summary>
    public int EconomyCapacity { get; set; }

    /// <summary>
    /// Statut de l'avion.
    /// Exemple : Available, Maintenance, OutOfService.
    /// </summary>
    [MaxLength(30)]
    public string Status { get; set; } = "Available";

    /// <summary>
    /// Date de dernière maintenance connue.
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }
}