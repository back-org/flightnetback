using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant un membre d'équipe ou d'équipage.
/// Il complète les informations d'un utilisateur interne de l'application.
/// </summary>
public class CrewMemberDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO membre d'équipe.
    /// </summary>
    public CrewMemberDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO membre d'équipe.
    /// </summary>
    public CrewMemberDto(
        int id,
        int userId,
        string employeeNumber,
        string position,
        string licenseNumber,
        DateTime hireDate,
        string status)
    {
        Id = id;
        UserId = userId;
        EmployeeNumber = employeeNumber;
        Position = position;
        LicenseNumber = licenseNumber;
        HireDate = hireDate;
        Status = status;
    }

    /// <summary>
    /// Identifiant unique du membre d'équipe.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de l'utilisateur lié à ce membre d'équipe.
    /// </summary>
    [Required(ErrorMessage = "L'identifiant utilisateur est requis.")]
    public int UserId { get; set; }

    /// <summary>
    /// Numéro matricule ou identifiant employé.
    /// </summary>
    [Required(ErrorMessage = "Le numéro matricule est requis.")]
    [MaxLength(30, ErrorMessage = "Le numéro matricule ne peut pas dépasser 30 caractères.")]
    public string EmployeeNumber { get; set; } = string.Empty;

    /// <summary>
    /// Poste ou fonction occupé(e).
    /// Exemple : Pilot, CoPilot, CabinCrew, Supervisor.
    /// </summary>
    [Required(ErrorMessage = "La fonction est requise.")]
    [MaxLength(50, ErrorMessage = "La fonction ne peut pas dépasser 50 caractères.")]
    public string Position { get; set; } = string.Empty;

    /// <summary>
    /// Numéro de licence professionnelle si applicable.
    /// </summary>
    [MaxLength(50, ErrorMessage = "Le numéro de licence ne peut pas dépasser 50 caractères.")]
    public string LicenseNumber { get; set; } = string.Empty;

    /// <summary>
    /// Date d'embauche.
    /// </summary>
    public DateTime HireDate { get; set; }

    /// <summary>
    /// Statut du membre d'équipe.
    /// Exemple : Active, OnLeave, Suspended.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "Active";
}