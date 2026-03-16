using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant une notification destinée à un utilisateur.
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO notification.
    /// </summary>
    public NotificationDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO notification avec ses valeurs.
    /// </summary>
    public NotificationDto(
        int id,
        int userId,
        string subject,
        string message,
        string channel,
        string status,
        DateTime createdAt,
        DateTime? sentAt)
    {
        Id = id;
        UserId = userId;
        Subject = subject;
        Message = message;
        Channel = channel;
        Status = status;
        CreatedAt = createdAt;
        SentAt = sentAt;
    }

    /// <summary>
    /// Identifiant unique de la notification.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de l'utilisateur destinataire.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant utilisateur est requis.")]
    public int UserId { get; set; }

    /// <summary>
    /// Sujet de la notification.
    /// </summary>
    [MaxLength(100, ErrorMessage = "Le sujet ne peut pas dépasser 100 caractères.")]
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Contenu du message de notification.
    /// </summary>
    [Required(ErrorMessage = "Le message est requis.")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Canal d'envoi.
    /// Exemple : Email, SMS, InApp.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le canal ne peut pas dépasser 30 caractères.")]
    public string Channel { get; set; } = "InApp";

    /// <summary>
    /// Statut de la notification.
    /// Exemple : Pending, Sent, Failed, Read.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Date de création de la notification.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date d'envoi de la notification, si elle a été envoyée.
    /// </summary>
    public DateTime? SentAt { get; set; }
}