using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant un utilisateur de l'application.
/// Il sert à transporter les données utilisateur entre les couches
/// sans exposer directement l'entité métier.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO utilisateur.
    /// </summary>
    public UserDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO utilisateur avec ses valeurs principales.
    /// </summary>
    public UserDto(
        int id,
        string userName,
        string email,
        string firstName,
        string lastName,
        string phoneNumber,
        bool isActive,
        DateTime createdAt,
        DateTime? updatedAt,
        DateTime? lastLoginAt)
    {
        Id = id;
        UserName = userName;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        LastLoginAt = lastLoginAt;
    }

    /// <summary>
    /// Identifiant unique de l'utilisateur.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom d'utilisateur utilisé pour la connexion.
    /// </summary>
    [Required(ErrorMessage = "Le nom d'utilisateur est requis.")]
    [MaxLength(50, ErrorMessage = "Le nom d'utilisateur ne peut pas dépasser 50 caractères.")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Adresse e-mail principale de l'utilisateur.
    /// </summary>
    [Required(ErrorMessage = "L'adresse e-mail est requise.")]
    [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
    [MaxLength(100, ErrorMessage = "L'adresse e-mail ne peut pas dépasser 100 caractères.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Prénom de l'utilisateur.
    /// </summary>
    [MaxLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères.")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Nom de famille de l'utilisateur.
    /// </summary>
    [MaxLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Numéro de téléphone de l'utilisateur.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le numéro de téléphone ne peut pas dépasser 30 caractères.")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Indique si le compte utilisateur est actif.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date de création du compte.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date de dernière modification.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Date de dernière connexion connue.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
}