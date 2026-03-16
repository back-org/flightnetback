/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant un avion utilisé dans l'application.
/// Il contient les informations principales sur un appareil de la flotte.
/// </summary>
public class AircraftDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide du DTO avion.
    /// </summary>
    public AircraftDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance du DTO avion.
    /// </summary>
    public AircraftDto(
        int id,
        string registrationNumber,
        string manufacturer,
        string model,
        int businessCapacity,
        int economyCapacity,
        string status,
        DateTime? lastMaintenanceDate)
    {
        Id = id;
        RegistrationNumber = registrationNumber;
        Manufacturer = manufacturer;
        Model = model;
        BusinessCapacity = businessCapacity;
        EconomyCapacity = economyCapacity;
        Status = status;
        LastMaintenanceDate = lastMaintenanceDate;
    }

    /// <summary>
    /// Identifiant unique de l'avion.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Numéro d'immatriculation de l'avion.
    /// Exemple : 5R-ABC
    /// </summary>
    [Required(ErrorMessage = "Le numéro d'immatriculation est requis.")]
    [MaxLength(30, ErrorMessage = "Le numéro d'immatriculation ne peut pas dépasser 30 caractères.")]
    public string RegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Fabricant de l'avion.
    /// Exemple : Airbus, Boeing.
    /// </summary>
    [MaxLength(50, ErrorMessage = "Le fabricant ne peut pas dépasser 50 caractères.")]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Modèle de l'avion.
    /// Exemple : A320, B737.
    /// </summary>
    [MaxLength(50, ErrorMessage = "Le modèle ne peut pas dépasser 50 caractères.")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Nombre de sièges en classe affaires.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "La capacité business doit être positive.")]
    public int BusinessCapacity { get; set; }

    /// <summary>
    /// Nombre de sièges en classe économique.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "La capacité economy doit être positive.")]
    public int EconomyCapacity { get; set; }

    /// <summary>
    /// Statut de l'avion.
    /// Exemple : Available, Maintenance, OutOfService.
    /// </summary>
    [MaxLength(30, ErrorMessage = "Le statut ne peut pas dépasser 30 caractères.")]
    public string Status { get; set; } = "Available";

    /// <summary>
    /// Date de dernière maintenance connue.
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }
}