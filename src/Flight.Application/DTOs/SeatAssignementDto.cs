/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant l'attribution d'un siège à un passager pour un vol donné.
/// </summary>
public class SeatAssignmentDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO attribution de siège.
    /// </summary>
    public SeatAssignmentDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO attribution de siège avec ses valeurs.
    /// </summary>
    public SeatAssignmentDto(
        int id,
        int flightId,
        int passengerId,
        string seatNumber,
        string seatClass)
    {
        Id = id;
        FlightId = flightId;
        PassengerId = passengerId;
        SeatNumber = seatNumber;
        SeatClass = seatClass;
    }

    /// <summary>
    /// Identifiant unique de l'attribution de siège.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifiant du vol concerné.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant du vol est requis.")]
    public int FlightId { get; set; }

    /// <summary>
    /// Identifiant du passager concerné.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant du passager est requis.")]
    public int PassengerId { get; set; }

    /// <summary>
    /// Numéro ou code du siège.
    /// Exemple : 12A
    /// </summary>
    [Required(ErrorMessage = "Le numéro du siège est requis.")]
    [MaxLength(10, ErrorMessage = "Le numéro du siège ne peut pas dépasser 10 caractères.")]
    public string SeatNumber { get; set; } = string.Empty;

    /// <summary>
    /// Classe du siège.
    /// Exemple : Economy, Business.
    /// </summary>
    [MaxLength(30, ErrorMessage = "La classe du siège ne peut pas dépasser 30 caractères.")]
    public string SeatClass { get; set; } = "Economy";
}