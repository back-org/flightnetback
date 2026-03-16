/*
 * Rôle métier du fichier: Modéliser les entités métier du domaine aérien.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain/Entities' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un utilisateur pouvant se connecter à l'application.
/// Cet utilisateur peut être un administrateur, un agent, un membre d'équipage
/// ou même un passager disposant d'un compte.
/// </summary>
[Table("Users")]
public class User
{
    /// <summary>
    /// Identifiant unique de l'utilisateur.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nom d'utilisateur utilisé pour se connecter.
    /// Exemple : patrick.ranoelison
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Adresse e-mail principale de l'utilisateur.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Mot de passe chiffré (hashé).
    /// On ne stocke jamais le mot de passe en clair.
    /// </summary>
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Prénom de l'utilisateur.
    /// </summary>
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Nom de famille de l'utilisateur.
    /// </summary>
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Numéro de téléphone de l'utilisateur.
    /// </summary>
    [MaxLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Indique si le compte utilisateur est actif.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date de création du compte.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date de dernière mise à jour du compte.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Date de dernière connexion connue.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
}