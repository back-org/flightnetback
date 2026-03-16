/*
 * Rôle métier du fichier: Modéliser les entités métier du domaine aérien.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain/Entities' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un membre de l'équipe ou de l'équipage.
/// Cette entité complète les informations d'un utilisateur interne.
/// </summary>
[Table("CrewMembers")]
public class CrewMember
{
    /// <summary>
    /// Identifiant unique du membre d'équipage.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Référence vers le compte utilisateur correspondant.
    /// </summary>
    [Required]
    public int UserId { get; set; }

    /// <summary>
    /// Numéro matricule ou identifiant employé.
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string EmployeeNumber { get; set; } = string.Empty;

    /// <summary>
    /// Fonction du membre d'équipe.
    /// Exemple : Pilot, CoPilot, CabinCrew, Supervisor.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Numéro de licence professionnelle si applicable.
    /// Utile notamment pour les pilotes.
    /// </summary>
    [MaxLength(50)]
    public string LicenseNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date d'embauche.
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// Statut du membre d'équipe.
    /// Exemple : Active, OnLeave, Suspended.
    /// </summary>
    [MaxLength(30)]
    public string Status { get; set; } = "Active";
}