using System.ComponentModel.DataAnnotations;
using Flight.Domain.Entities;

namespace Flight.Application.DTOs;

/// <summary>
/// Représente les données transférées pour un passager.
/// Ce DTO est utilisé pour la lecture, la création et la mise à jour
/// des passagers via l'API.
/// </summary>
public class PassengerDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="PassengerDto"/>.
    /// Ce constructeur est nécessaire pour la sérialisation et la désérialisation JSON.
    /// </summary>
    public PassengerDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="PassengerDto"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique du passager.</param>
    /// <param name="name">Prénom du passager.</param>
    /// <param name="middleName">Deuxième prénom du passager.</param>
    /// <param name="lastName">Nom de famille du passager.</param>
    /// <param name="email">Adresse e-mail du passager.</param>
    /// <param name="contact">Numéro de contact du passager.</param>
    /// <param name="address">Adresse postale du passager.</param>
    /// <param name="sex">Genre du passager.</param>
    public PassengerDto(
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
    /// Obtient ou définit l'identifiant unique du passager.
    /// Pour une création, cette valeur peut être à 0.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Obtient ou définit le prénom du passager.
    /// </summary>
    [Required(ErrorMessage = "Le prénom du passager est requis.")]
    [MaxLength(50, ErrorMessage = "Le prénom du passager ne peut pas dépasser 50 caractères.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le deuxième prénom du passager.
    /// Ce champ est optionnel.
    /// </summary>
    [MaxLength(100, ErrorMessage = "Le deuxième prénom ne peut pas dépasser 100 caractères.")]
    public string MiddleName { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le nom de famille du passager.
    /// </summary>
    [Required(ErrorMessage = "Le nom de famille du passager est requis.")]
    [MaxLength(100, ErrorMessage = "Le nom de famille du passager ne peut pas dépasser 100 caractères.")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit l'adresse e-mail du passager.
    /// </summary>
    [Required(ErrorMessage = "L'adresse e-mail du passager est requise.")]
    [EmailAddress(ErrorMessage = "L'adresse e-mail du passager est invalide.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le numéro de contact du passager.
    /// </summary>
    [Required(ErrorMessage = "Le contact du passager est requis.")]
    public string Contact { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit l'adresse postale du passager.
    /// </summary>
    [Required(ErrorMessage = "L'adresse du passager est requise.")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit le genre du passager.
    /// </summary>
    [Required(ErrorMessage = "Le genre du passager est requis.")]
    public Genre Sex { get; set; } = Genre.Unknown;
}