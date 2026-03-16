/*
 * Rôle métier du fichier: Modéliser les entités métier du domaine aérien.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain/Entities' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente une notification destinée à un utilisateur.
/// Une notification peut être envoyée par e-mail, SMS ou rester interne à l'application.
/// </summary>
[Table("Notifications")]
public class Notification
{
    /// <summary>
    /// Identifiant unique de la notification.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de l'utilisateur destinataire.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant utilisateur est requis.")]
    public int UserId { get; set; }

    /// <summary>
    /// Sujet ou titre de la notification.
    /// </summary>
    [MaxLength(100, ErrorMessage = "Le sujet ne peut pas dépasser 100 caractères.")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Contenu détaillé du message à transmettre.
    /// </summary>
    [Required(ErrorMessage = "Le message est requis.")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Canal utilisé pour la notification.
    /// Exemple : Email, SMS, InApp.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le canal ne peut pas dépasser 30 caractères.")]
    public string Channel { get; set; } = "InApp";

    /// <summary>
    /// Statut d'envoi ou de lecture de la notification.
    /// Exemple : Pending, Sent, Failed, Read.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Date de création de la notification.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date à laquelle la notification a réellement été envoyée.
    /// </summary>
    public DateTime? SentAt { get; set; }
}