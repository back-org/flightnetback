using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Abstracts;
using Newtonsoft.Json;

namespace Flight.Domain.Entities;

/// <summary>
/// Extensions de mapping pour l'entité <see cref="Passenger"/>.
/// </summary>
public static class PassengerExtensions
{
    /// <summary>
    /// Convertit une entité <see cref="Passenger"/> en <see cref="PassengerDto"/>.
    /// </summary>
    /// <param name="passenger">L'entité passager à convertir.</param>
    /// <returns>Le DTO correspondant.</returns>
    public static PassengerDto ToDto(this Passenger passenger)
    {
        return new PassengerDto(
            passenger.Id,
            passenger.Name,
            passenger.MiddleName,
            passenger.LastName,
            passenger.Email,
            passenger.Contact,
            passenger.Address,
            passenger.Sex);
    }
}

/// <summary>
/// Objet de transfert de données (DTO) pour un passager.
/// Utilisé pour les opérations de création et mise à jour via l'API.
/// </summary>
/// <param name="Id">Identifiant du passager (0 pour une création).</param>
/// <param name="Name">Prénom du passager.</param>
/// <param name="MiddleName">Deuxième prénom (optionnel).</param>
/// <param name="LastName">Nom de famille du passager.</param>
/// <param name="Email">Adresse e-mail du passager.</param>
/// <param name="Contact">Numéro de contact du passager.</param>
/// <param name="Address">Adresse postale du passager.</param>
/// <param name="Sex">Genre du passager.</param>
public record PassengerDto(
    int Id,
    string Name,
    string MiddleName,
    string LastName,
    string Email,
    string Contact,
    string Address,
    Genre Sex);

/// <summary>
/// Représente un passager dans le système de réservation.
/// Contient les informations personnelles et de contact du passager.
/// </summary>
[Table("Passengers")]
public class Passenger : DeleteEntity<int>
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="Passenger"/>.
    /// </summary>
    public Passenger()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="Passenger"/> à partir d'un DTO.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données du passager.</param>
    public Passenger(PassengerDto dto)
    {
        Copy(dto);
    }

    /// <summary>
    /// Prénom du passager.
    /// </summary>
    [Required(ErrorMessage = "Le prénom du passager est requis.")]
    [MaxLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères.")]
    [Column("name")]
    [JsonProperty(PropertyName = "name")]
    [DataType(DataType.Text)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Deuxième prénom du passager (optionnel).
    /// </summary>
    [MaxLength(100, ErrorMessage = "Le deuxième prénom ne peut pas dépasser 100 caractères.")]
    [Column("middle_name")]
    [JsonProperty(PropertyName = "middle_name")]
    [DataType(DataType.Text)]
    public string MiddleName { get; set; } = string.Empty;

    /// <summary>
    /// Nom de famille du passager.
    /// </summary>
    [Required(ErrorMessage = "Le nom de famille est requis.")]
    [MaxLength(100, ErrorMessage = "Le nom de famille ne peut pas dépasser 100 caractères.")]
    [Column("last_name")]
    [JsonProperty(PropertyName = "last_name")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Adresse e-mail du passager.
    /// </summary>
    [EmailAddress(ErrorMessage = "L'adresse e-mail est invalide.")]
    [Required(ErrorMessage = "L'adresse e-mail est requise.")]
    [Column("email")]
    [JsonProperty(PropertyName = "email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Numéro de téléphone ou autre moyen de contact du passager.
    /// </summary>
    [Required(ErrorMessage = "Le contact est requis.")]
    [Column("contact")]
    [JsonProperty(PropertyName = "contact")]
    [DataType(DataType.Text)]
    public string Contact { get; set; } = string.Empty;

    /// <summary>
    /// Adresse postale du passager.
    /// </summary>
    [Required(ErrorMessage = "L'adresse est requise.")]
    [Column("address")]
    [JsonProperty(PropertyName = "address")]
    [DataType(DataType.Text)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Genre du passager (Masculin, Féminin, Inconnu).
    /// </summary>
    [Required(ErrorMessage = "Le genre est requis.")]
    [Column("genre")]
    [JsonProperty(PropertyName = "genre")]
    public Genre Sex { get; set; } = Genre.Unknown;

    /// <summary>
    /// Copie les valeurs d'un <see cref="PassengerDto"/> dans cette entité.
    /// </summary>
    /// <param name="dto">Le DTO source contenant les nouvelles valeurs.</param>
    public void Copy(PassengerDto dto)
    {
        Id = dto.Id > 0 ? dto.Id : 0;
        Name = dto.Name;
        MiddleName = dto.MiddleName;
        LastName = dto.LastName;
        Email = dto.Email;
        Contact = dto.Contact;
        Address = dto.Address;
        Sex = dto.Sex;
    }
}
