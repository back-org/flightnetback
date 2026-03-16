using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente une trace d'audit enregistrée par le système.
/// Permet de savoir qui a fait quoi, quand et depuis quelle adresse IP.
/// </summary>
[Table("AuditLogs")]
public class AuditLog
{
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Action effectuée.
    /// Exemple : CREATE, UPDATE, DELETE, LOGIN.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Nom de l'entité concernée.
    /// Exemple : Flight, Booking, User.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// Identifiant texte de l'entité concernée.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string EntityId { get; set; } = string.Empty;

    /// <summary>
    /// Détails additionnels sur l'action.
    /// </summary>
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// Utilisateur ayant effectué l'action.
    /// </summary>
    [MaxLength(100)]
    public string PerformedBy { get; set; } = string.Empty;

    /// <summary>
    /// Adresse IP source de l'action.
    /// </summary>
    [MaxLength(50)]
    public string IpAddress { get; set; } = string.Empty;

    /// <summary>
    /// Date et heure de l'action.
    /// </summary>
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indique si l'action a réussi.
    /// </summary>
    public bool Success { get; set; } = true;
}