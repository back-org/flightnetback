using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// Représente les données transférées pour un véhicule.
/// Ce DTO est utilisé pour la lecture, la création et la mise à jour
/// des véhicules via l'API.
/// </summary>
public class VehicleDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="VehicleDto"/>.
    /// Ce constructeur est nécessaire pour la sérialisation et la désérialisation JSON.
    /// </summary>
    public VehicleDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="VehicleDto"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique du véhicule.</param>
    /// <param name="licensePlate">Plaque d'immatriculation du véhicule.</param>
    /// <param name="manufacturer">Fabricant ou marque du véhicule.</param>
    /// <param name="model">Modèle du véhicule.</param>
    /// <param name="year">Année de fabrication du véhicule.</param>
    /// <param name="tariff">Tarif appliqué au véhicule.</param>
    public VehicleDto(
        int id,
        string licensePlate,
        string manufacturer,
        string model,
        short year,
        float tariff)
    {
        Id = id;
        LicensePlate = licensePlate;
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        Tariff = tariff;
    }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du véhicule.
    /// Pour une création, cette valeur peut être à 0.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Obtient ou définit la plaque d'immatriculation du véhicule.
    /// </summary>
    [Required(ErrorMessage = "La plaque d'immatriculation est requise.")]
    [MaxLength(20, ErrorMessage = "La plaque d'immatriculation ne peut pas dépasser 20 caractères.")]
    public string LicensePlate { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le fabricant ou la marque du véhicule.
    /// </summary>
    [Required(ErrorMessage = "Le fabricant du véhicule est requis.")]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le modèle du véhicule.
    /// </summary>
    [Required(ErrorMessage = "Le modèle du véhicule est requis.")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit l'année de fabrication du véhicule.
    /// </summary>
    [Range(1900, 2100, ErrorMessage = "L'année de fabrication doit être comprise entre 1900 et 2100.")]
    public short Year { get; set; }

    /// <summary>
    /// Obtient ou définit le tarif appliqué au véhicule.
    /// </summary>
    [Range(0, float.MaxValue, ErrorMessage = "Le tarif du véhicule doit être positif ou nul.")]
    public float Tariff { get; set; }
}