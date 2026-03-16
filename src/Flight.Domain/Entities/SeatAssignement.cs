/*
 * Rôle métier du fichier: Modéliser les entités métier du domaine aérien.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain/Entities' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente l'attribution d'un siège à un passager pour un vol donné.
/// Cette entité permet de savoir quel passager occupe quel siège.
/// </summary>
[Table("SeatAssignments")]
public class SeatAssignment
{
    /// <summary>
    /// Identifiant unique de l'attribution de siège.
    /// </summary>
    [Key]
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
    /// Numéro ou code du siège attribué.
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