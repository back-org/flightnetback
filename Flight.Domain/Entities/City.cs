using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Extensions de mapping pour l'entité <see cref="City"/>.
/// </summary>
public static class CityExtensions
{
    /// <summary>
    /// Convertit une entité <see cref="City"/> en <see cref="CityDto"/>.
    /// </summary>
    /// <param name="city">L'entité ville à convertir.</param>
    /// <returns>Le DTO correspondant.</returns>
    public static CityDto ToDto(this City city)
    {
        return new CityDto(city.Id, city.Name, city.Lat, city.Lon, city.CountryId);
    }
}

/// <summary>
/// Objet de transfert de données (DTO) pour une ville.
/// Utilisé pour les opérations de création et mise à jour via l'API.
/// </summary>
/// <param name="Id">Identifiant de la ville (0 pour une création).</param>
/// <param name="Name">Nom de la ville.</param>
/// <param name="Lat">Latitude géographique de la ville.</param>
/// <param name="Lon">Longitude géographique de la ville.</param>
/// <param name="CountryId">Identifiant du pays auquel appartient la ville.</param>
public record CityDto(int Id, string Name, decimal Lat, decimal Lon, int CountryId);

/// <summary>
/// Représente une ville dans le système.
/// Une ville est rattachée à un <see cref="Country"/> et possède des coordonnées géographiques.
/// </summary>
[Table("Cities")]
public class City : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance de <see cref="City"/> à partir d'un DTO.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données de la ville.</param>
    public City(CityDto dto)
    {
        Copy(dto);
    }

    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="City"/>.
    /// </summary>
    public City()
    {
    }

    /// <summary>
    /// Copie les valeurs d'un <see cref="CityDto"/> dans cette entité.
    /// </summary>
    /// <param name="dto">Le DTO source contenant les nouvelles valeurs.</param>
    public void Copy(CityDto dto)
    {
        Id = dto.Id > 0 ? dto.Id : 0;
        Name = dto.Name;
        Lat = dto.Lat;
        Lon = dto.Lon;
        CountryId = dto.CountryId;
    }

    /// <summary>
    /// Nom de la ville en format UTF-8.
    /// </summary>
    [Required(ErrorMessage = "Le nom de la ville est requis.")]
    [MaxLength(30, ErrorMessage = "Le nom ne peut pas dépasser 30 caractères.")]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Latitude géographique de la ville (format décimal, précision 7,4).
    /// </summary>
    [JsonProperty(PropertyName = "lat")]
    [Column("lat", TypeName = "decimal(7,4)")]
    public decimal Lat { get; set; }

    /// <summary>
    /// Longitude géographique de la ville (format décimal, précision 7,4).
    /// </summary>
    [Column("lon", TypeName = "decimal(7,4)")]
    [JsonProperty(PropertyName = "lon")]
    public decimal Lon { get; set; }

    /// <summary>
    /// Identifiant du pays auquel appartient cette ville (clé étrangère vers <see cref="Country"/>).
    /// </summary>
    [Column("country_id")]
    [JsonProperty(PropertyName = "country_id")]
    public int CountryId { get; set; }

    /// <summary>
    /// Navigation vers l'entité <see cref="Country"/> parente.
    /// </summary>
    [ForeignKey(nameof(CountryId))]
    public virtual Country? Country { get; set; }
}
