/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant un billet émis pour une réservation.
/// Ce DTO sert à transporter les informations du billet entre les couches de l'application.
/// </summary>
public class TicketDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO billet.
    /// </summary>
    public TicketDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO billet avec ses valeurs principales.
    /// </summary>
    public TicketDto(
        int id,
        string ticketNumber,
        int bookingId,
        int passengerId,
        DateTime issuedAt,
        string status)
    {
        Id = id;
        TicketNumber = ticketNumber;
        BookingId = bookingId;
        PassengerId = passengerId;
        IssuedAt = issuedAt;
        Status = status;
    }

    /// <summary>
    /// Identifiant unique du billet.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Numéro unique du billet.
    /// Exemple : TCK-2026-0001
    /// </summary>
    [Required(ErrorMessage = "Le numéro du billet est requis.")]
    [MaxLength(50, ErrorMessage = "Le numéro du billet ne peut pas dépasser 50 caractères.")]
    public string TicketNumber { get; set; } = string.Empty;

    /// <summary>
    /// Identifiant de la réservation liée à ce billet.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant de la réservation est requis.")]
    public int BookingId { get; set; }

    /// <summary>
    /// Identifiant du passager concerné par le billet.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant du passager est requis.")]
    public int PassengerId { get; set; }

    /// <summary>
    /// Date d'émission du billet.
    /// </summary>
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Statut du billet.
    /// Exemple : Issued, Cancelled, Used.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "Issued";
}