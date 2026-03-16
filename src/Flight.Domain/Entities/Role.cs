using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un rôle fonctionnel dans l'application.
/// Un rôle définit un groupe de responsabilités ou de permissions.
/// </summary>
[Table("Roles")]
public class Role
{
    /// <summary>
    /// Identifiant unique du rôle.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nom du rôle.
    /// Exemple : Admin, Passenger, BookingAgent.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description lisible du rôle.
    /// </summary>
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;
}