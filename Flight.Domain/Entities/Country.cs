using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Extensions de mapping pour l'entité <see cref="Country"/>.
/// </summary>
public static class CountryExtensions
{
    /// <summary>
    /// Convertit une entité <see cref="Country"/> en <see cref="CountryDto"/>.
    /// </summary>
    /// <param name="country">L'entité pays à convertir.</param>
    /// <returns>Le DTO correspondant.</returns>
    public static CountryDto ToDto(this Country country)
    {
        return new CountryDto(country.Id, country.Name, country.Iso2, country.Iso3);
    }
}

/// <summary>
/// Objet de transfert de données (DTO) pour un pays.
/// Utilisé pour les opérations de création et mise à jour via l'API.
/// </summary>
/// <param name="Id">Identifiant du pays (0 pour une création).</param>
/// <param name="Name">Nom officiel du pays.</param>
/// <param name="Iso2">Code ISO 3166-1 Alpha-2 (ex: FR, US, MG).</param>
/// <param name="Iso3">Code ISO 3166-1 Alpha-3 (ex: FRA, USA, MDG).</param>
public record CountryDto(int Id, string Name, string Iso2, string Iso3);

/// <summary>
/// Représente un pays dans le système géographique.
/// Un pays peut contenir plusieurs <see cref="City"/>.
/// </summary>
[Table("Countries")]
public class Country : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Country"/>.
    /// </summary>
    public Country()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Country"/> à partir d'un DTO.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données du pays.</param>
    public Country(CountryDto dto)
    {
        Copy(dto);
    }

    /// <summary>
    /// Copie les valeurs d'un <see cref="CountryDto"/> dans cette entité.
    /// </summary>
    /// <param name="dto">Le DTO source contenant les nouvelles valeurs.</param>
    public void Copy(CountryDto dto)
    {
        Id = dto.Id > 0 ? dto.Id : 0;
        Name = dto.Name;
        Iso2 = dto.Iso2;
        Iso3 = dto.Iso3;
    }

    /// <summary>
    /// Nom officiel du pays en format UTF-8.
    /// </summary>
    [Required(ErrorMessage = "Le nom du pays est requis.")]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Code ISO 3166-1 Alpha-2 du pays (2 lettres, ex : FR, US).
    /// </summary>
    [Required(ErrorMessage = "Le code ISO2 est requis.")]
    [MaxLength(2, ErrorMessage = "Le code ISO2 doit faire exactement 2 caractères.")]
    [Column("iso2")]
    [JsonProperty(PropertyName = "iso2")]
    [DataType(DataType.Text)]
    public string Iso2 { get; set; } = null!;

    /// <summary>
    /// Code ISO 3166-1 Alpha-3 du pays (3 lettres, ex : FRA, USA).
    /// </summary>
    [Required(ErrorMessage = "Le code ISO3 est requis.")]
    [MaxLength(3, ErrorMessage = "Le code ISO3 doit faire exactement 3 caractères.")]
    [Column("iso3")]
    [JsonProperty(PropertyName = "iso3")]
    [DataType(DataType.Text)]
    public string Iso3 { get; set; } = null!;

    /// <summary>
    /// Collection de villes appartenant à ce pays.
    /// </summary>
    public virtual ICollection<City> Cities { get; set; } = [];
}
