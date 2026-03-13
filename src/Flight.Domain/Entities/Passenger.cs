using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Représente un passager dans le système.
/// </summary>
[Table("Passengers")]
public partial class Passenger : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Passenger"/>.
    /// </summary>
    public Passenger()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Passenger"/> avec les valeurs fournies.
    /// </summary>
    public Passenger(
        int id,
        string name,
        string middleName,
        string lastName,
        string email,
        string contact,
        string address,
        Genre sex)
    {
        Id = id;
        Name = name;
        MiddleName = middleName;
        LastName = lastName;
        Email = email;
        Contact = contact;
        Address = address;
        Sex = sex;
    }

    /// <summary>
    /// Obtient ou définit le prénom du passager.
    /// </summary>
    [Required]
    [MaxLength(50)]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le deuxième prénom du passager.
    /// </summary>
    [MaxLength(100)]
    [Column("middle_name")]
    [JsonProperty(PropertyName = "middleName")]
    public string MiddleName { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le nom de famille du passager.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [Column("last_name")]
    [JsonProperty(PropertyName = "lastName")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit l'adresse e-mail du passager.
    /// </summary>
    [Required]
    [EmailAddress]
    [Column("email")]
    [JsonProperty(PropertyName = "email")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le numéro de contact du passager.
    /// </summary>
    [Required]
    [Column("contact")]
    [JsonProperty(PropertyName = "contact")]
    public string Contact { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit l'adresse postale du passager.
    /// </summary>
    [Required]
    [Column("address")]
    [JsonProperty(PropertyName = "address")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le genre du passager.
    /// </summary>
    [Column("sex")]
    [JsonProperty(PropertyName = "sex")]
    public Genre Sex { get; set; } = Genre.Unknown;
}