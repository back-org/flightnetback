using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un pays dans le système.
/// </summary>
[Table("Countries")]
public partial class Country : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Country"/>.
    /// </summary>
    public Country()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Country"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique du pays.</param>
    /// <param name="name">Nom officiel du pays.</param>
    /// <param name="iso2">Code ISO Alpha-2.</param>
    /// <param name="iso3">Code ISO Alpha-3.</param>
    public Country(int id, string name, string iso2, string iso3)
    {
        Id = id;
        Name = name;
        Iso2 = iso2;
        Iso3 = iso3;
    }

    /// <summary>
    /// Obtient ou définit le nom officiel du pays.
    /// </summary>
    [Required(ErrorMessage = "Le nom du pays est requis.")]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le code ISO 3166-1 Alpha-2 du pays.
    /// </summary>
    [Required(ErrorMessage = "Le code ISO2 est requis.")]
    [MaxLength(2)]
    [Column("iso2")]
    [JsonProperty(PropertyName = "iso2")]
    public string Iso2 { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le code ISO 3166-1 Alpha-3 du pays.
    /// </summary>
    [Required(ErrorMessage = "Le code ISO3 est requis.")]
    [MaxLength(3)]
    [Column("iso3")]
    [JsonProperty(PropertyName = "iso3")]
    public string Iso3 { get; set; } = string.Empty;

    /// <summary>
    /// Collection de villes appartenant à ce pays.
    /// </summary>
    public virtual ICollection<City> Cities { get; set; } = [];
}