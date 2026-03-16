/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant un rôle dans l'application.
/// Un rôle permet de regrouper des autorisations ou des responsabilités.
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO rôle.
    /// </summary>
    public RoleDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO rôle avec ses valeurs.
    /// </summary>
    public RoleDto(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Identifiant unique du rôle.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom du rôle.
    /// Exemple : Admin, Passenger, BookingAgent.
    /// </summary>
    [Required(ErrorMessage = "Le nom du rôle est requis.")]
    [MaxLength(50, ErrorMessage = "Le nom du rôle ne peut pas dépasser 50 caractères.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description du rôle.
    /// </summary>
    [MaxLength(200, ErrorMessage = "La description ne peut pas dépasser 200 caractères.")]
    public string Description { get; set; } = string.Empty;
}