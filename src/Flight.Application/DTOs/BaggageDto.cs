/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant un bagage associé à un passager et à un vol.
/// </summary>
public class BaggageDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO bagage.
    /// </summary>
    public BaggageDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO bagage avec ses valeurs.
    /// </summary>
    public BaggageDto(
        int id,
        int bookingId,
        int passengerId,
        int flightId,
        string tagNumber,
        decimal weight,
        string type,
        string status)
    {
        Id = id;
        BookingId = bookingId;
        PassengerId = passengerId;
        FlightId = flightId;
        TagNumber = tagNumber;
        Weight = weight;
        Type = type;
        Status = status;
    }

    /// <summary>
    /// Identifiant unique du bagage.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de la réservation liée.
    /// </summary>
    public int BookingId { get; set; }

    /// <summary>
    /// Identifiant du passager.
    /// </summary>
    public int PassengerId { get; set; }

    /// <summary>
    /// Identifiant du vol concerné.
    /// </summary>
    public int FlightId { get; set; }

    /// <summary>
    /// Numéro d'étiquette du bagage.
    /// </summary>
    [MaxLength(50, ErrorMessage = "Le numéro d'étiquette ne peut pas dépasser 50 caractères.")]
    public string TagNumber { get; set; } = string.Empty;

    /// <summary>
    /// Poids du bagage en kilogrammes.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Le poids doit être positif ou nul.")]
    public decimal Weight { get; set; }

    /// <summary>
    /// Type de bagage.
    /// Exemple : Cabin, Checked.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le type ne peut pas dépasser 30 caractères.")]
    public string Type { get; set; } = "Checked";

    /// <summary>
    /// Statut du bagage.
    /// Exemple : CheckedIn, Loaded, Delivered.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "CheckedIn";
}