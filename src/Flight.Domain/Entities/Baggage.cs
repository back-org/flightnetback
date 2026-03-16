using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un bagage lié à un passager, une réservation et un vol.
/// Cette entité permet de suivre les bagages enregistrés ou cabine.
/// </summary>
[Table("Baggages")]
public class Baggage
{
    /// <summary>
    /// Identifiant unique du bagage.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de la réservation associée.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant de la réservation est requis.")]
    public int BookingId { get; set; }

    /// <summary>
    /// Identifiant du passager auquel appartient le bagage.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant du passager est requis.")]
    public int PassengerId { get; set; }

    /// <summary>
    /// Identifiant du vol concerné.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant du vol est requis.")]
    public int FlightId { get; set; }

    /// <summary>
    /// Numéro d'étiquette du bagage.
    /// </summary>
    [MaxLength(50, ErrorMessage = "Le numéro d'étiquette ne peut pas dépasser 50 caractères.")]
    public string TagNumber { get; set; } = string.Empty;

    /// <summary>
    /// Poids du bagage en kilogrammes.
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// Type de bagage.
    /// Exemple : Cabin, Checked.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le type ne peut pas dépasser 30 caractères.")]
    public string Type { get; set; } = "Checked";

    /// <summary>
    /// Statut actuel du bagage.
    /// Exemple : CheckedIn, Loaded, Delivered, Lost.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "CheckedIn";
}