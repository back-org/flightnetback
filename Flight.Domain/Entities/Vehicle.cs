using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Extensions de mapping pour l'entité <see cref="Vehicle"/>.
/// </summary>
public static class VehicleExtensions
{
    /// <summary>
    /// Convertit une entité <see cref="Vehicle"/> en <see cref="VehicleDto"/>.
    /// </summary>
    /// <param name="vehicle">L'entité véhicule à convertir.</param>
    /// <returns>Le DTO correspondant.</returns>
    public static VehicleDto ToDto(this Vehicle vehicle)
    {
        return new VehicleDto(vehicle.Id, vehicle.LicensePlate, vehicle.Manufacturer, vehicle.Model, vehicle.Year, vehicle.Tariff);
    }
}

/// <summary>
/// Objet de transfert de données (DTO) pour un véhicule de transport.
/// Utilisé pour les opérations de création et mise à jour via l'API.
/// </summary>
/// <param name="Id">Identifiant du véhicule (0 pour une création).</param>
/// <param name="LicensePlate">Numéro d'immatriculation.</param>
/// <param name="Manufacturer">Fabricant du véhicule.</param>
/// <param name="Model">Modèle du véhicule.</param>
/// <param name="Year">Année de fabrication.</param>
/// <param name="Tariff">Tarif journalier ou horaire du véhicule.</param>
public record VehicleDto(int Id, string LicensePlate, string Manufacturer, string Model, short Year, float Tariff);

/// <summary>
/// Représente un véhicule de transport terrestre associé au système (navette, taxi aéroport, etc.).
/// </summary>
[Table("Vehicles")]
public class Vehicle : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Vehicle"/>.
    /// </summary>
    public Vehicle()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Vehicle"/> à partir d'un DTO.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données du véhicule.</param>
    public Vehicle(VehicleDto dto)
    {
        Copy(dto);
    }

    /// <summary>
    /// Numéro d'immatriculation du véhicule.
    /// </summary>
    [Required(ErrorMessage = "La plaque d'immatriculation est requise.")]
    [MaxLength(20, ErrorMessage = "La plaque ne peut pas dépasser 20 caractères.")]
    [Column("license")]
    [JsonProperty(PropertyName = "license")]
    [DataType(DataType.Text)]
    public string LicensePlate { get; set; } = string.Empty;

    /// <summary>
    /// Marque ou fabricant du véhicule (ex : Toyota, Mercedes).
    /// </summary>
    [Required(ErrorMessage = "Le fabricant est requis.")]
    [Column("manufacturer")]
    [JsonProperty(PropertyName = "manufacturer")]
    [DataType(DataType.Text)]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Modèle du véhicule (ex : Corolla, Sprinter).
    /// </summary>
    [Required(ErrorMessage = "Le modèle est requis.")]
    [Column("model")]
    [JsonProperty(PropertyName = "model")]
    [DataType(DataType.Text)]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Année de fabrication du véhicule.
    /// </summary>
    [Range(1900, 2100, ErrorMessage = "L'année doit être comprise entre 1900 et 2100.")]
    [Column("year")]
    [JsonProperty(PropertyName = "year")]
    public short Year { get; set; }

    /// <summary>
    /// Tarif de location du véhicule (par heure ou par trajet).
    /// </summary>
    [Range(0, float.MaxValue, ErrorMessage = "Le tarif doit être positif.")]
    [Column("tariff")]
    [JsonProperty(PropertyName = "tariff")]
    public float Tariff { get; set; }

    /// <summary>
    /// Copie les valeurs d'un <see cref="VehicleDto"/> dans cette entité.
    /// </summary>
    /// <param name="dto">Le DTO source contenant les nouvelles valeurs.</param>
    public void Copy(VehicleDto dto)
    {
        Id = dto.Id > 0 ? dto.Id : 0;
        LicensePlate = dto.LicensePlate;
        Manufacturer = dto.Manufacturer;
        Model = dto.Model;
        Year = dto.Year;
        Tariff = dto.Tariff;
    }
}
