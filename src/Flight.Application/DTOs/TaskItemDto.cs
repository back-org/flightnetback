using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant une tâche interne dans l'application.
/// Une tâche peut être attribuée à un utilisateur pour le suivi d'une action.
/// </summary>
public class TaskItemDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO tâche.
    /// </summary>
    public TaskItemDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO tâche avec ses valeurs principales.
    /// </summary>
    public TaskItemDto(
        int id,
        string title,
        string description,
        int createdByUserId,
        int? assignedToUserId,
        string priority,
        string status,
        DateTime? dueDate,
        DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedByUserId = createdByUserId;
        AssignedToUserId = assignedToUserId;
        Priority = priority;
        Status = status;
        DueDate = dueDate;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Identifiant unique de la tâche.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Titre court de la tâche.
    /// </summary>
    [Required(ErrorMessage = "Le titre de la tâche est requis.")]
    [MaxLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères.")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description détaillée de la tâche.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Identifiant de l'utilisateur qui a créé la tâche.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant du créateur est requis.")]
    public int CreatedByUserId { get; set; }

    /// <summary>
    /// Identifiant de l'utilisateur à qui la tâche est attribuée.
    /// Ce champ peut être vide si la tâche n'est pas encore assignée.
    /// </summary>
    public int? AssignedToUserId { get; set; }

    /// <summary>
    /// Niveau de priorité de la tâche.
    /// Exemple : Low, Medium, High.
    /// </summary>
    [MaxLength(20, ErrorMessage = "La priorité ne peut pas dépasser 20 caractères.")]
    public string Priority { get; set; } = "Medium";

    /// <summary>
    /// Statut de la tâche.
    /// Exemple : Open, InProgress, Done, Cancelled.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "Open";

    /// <summary>
    /// Date limite prévue pour la tâche.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Date de création de la tâche.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}