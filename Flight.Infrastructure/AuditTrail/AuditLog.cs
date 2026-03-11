using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Flight.Infrastructure.AuditTrail;

/// <summary>
/// Entité représentant une entrée dans la piste d'audit.
/// Enregistre toutes les opérations sensibles effectuées sur le système.
/// </summary>
[Table("AuditLogs")]
public class AuditLog
{
    /// <summary>Identifiant unique de l'entrée d'audit.</summary>
    [Key]
    public int Id { get; set; }

    /// <summary>Nom de l'action effectuée (CREATE, UPDATE, DELETE, LOGIN, etc.).</summary>
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    /// <summary>Nom de l'entité concernée par l'opération.</summary>
    [Required]
    [MaxLength(100)]
    public string EntityName { get; set; } = string.Empty;

    /// <summary>Identifiant de l'entité concernée.</summary>
    [MaxLength(100)]
    public string? EntityId { get; set; }

    /// <summary>Détails de l'opération effectuée.</summary>
    [MaxLength(2000)]
    public string? Details { get; set; }

    /// <summary>Nom de l'utilisateur ayant effectué l'opération.</summary>
    [MaxLength(200)]
    public string? PerformedBy { get; set; }

    /// <summary>Adresse IP depuis laquelle l'opération a été effectuée.</summary>
    [MaxLength(50)]
    public string? IpAddress { get; set; }

    /// <summary>Date et heure UTC de l'opération.</summary>
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Indique si l'opération a réussi.</summary>
    public bool Success { get; set; } = true;
}
