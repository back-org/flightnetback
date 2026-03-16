using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un billet émis pour une réservation.
/// Un billet est généralement généré après validation de la réservation
/// et éventuellement après confirmation du paiement.
/// </summary>
[Table("Tickets")]
public class Ticket
{
    /// <summary>
    /// Identifiant unique du billet.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Numéro unique du billet.
    /// Exemple : TCK-2026-0001
    /// </summary>
    [Required(ErrorMessage = "Le numéro du billet est requis.")]
    [MaxLength(50, ErrorMessage = "Le numéro du billet ne peut pas dépasser 50 caractères.")]
    public string TicketNumber { get; set; } = string.Empty;

    /// <summary>
    /// Identifiant de la réservation associée à ce billet.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant de la réservation est requis.")]
    public int BookingId { get; set; }

    /// <summary>
    /// Identifiant du passager concerné par ce billet.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant du passager est requis.")]
    public int PassengerId { get; set; }

    /// <summary>
    /// Date d'émission du billet.
    /// </summary>
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Statut actuel du billet.
    /// Exemple : Issued, Cancelled, Used.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut du billet ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "Issued";
}