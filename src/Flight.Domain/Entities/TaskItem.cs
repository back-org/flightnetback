/*
 * Rôle métier du fichier: Modéliser les entités métier du domaine aérien.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain/Entities' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente une tâche interne dans l'application.
/// Cette entité est utile pour le travail collaboratif entre agents et équipes.
/// </summary>
[Table("TaskItems")]
public class TaskItem
{
    /// <summary>
    /// Identifiant unique de la tâche.
    /// </summary>
    [Key]
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
    /// Peut être nul si la tâche n'est pas encore assignée.
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