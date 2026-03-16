/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// Représente les données transférées pour un vol.
/// Ce DTO est utilisé pour la lecture, la création et la mise à jour
/// des vols via l'API.
/// </summary>
public class FlightDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="FlightDto"/>.
    /// Ce constructeur est nécessaire pour la sérialisation et la désérialisation JSON.
    /// </summary>
    public FlightDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="FlightDto"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique du vol.</param>
    /// <param name="code">Code unique du vol.</param>
    /// <param name="departure">Date et heure de départ prévues.</param>
    /// <param name="estimatedArrival">Date et heure estimée d'arrivée.</param>
    /// <param name="businessClassSlots">Nombre de places disponibles en classe affaires.</param>
    /// <param name="economySlots">Nombre de places disponibles en classe économique.</param>
    /// <param name="businessClassPrice">Tarif de la classe affaires.</param>
    /// <param name="economyPrice">Tarif de la classe économique.</param>
    /// <param name="to">Identifiant de l'aéroport de destination.</param>
    /// <param name="from">Identifiant de l'aéroport de départ.</param>
    public FlightDto(
        int id,
        string code,
        DateTime departure,
        DateTime estimatedArrival,
        int businessClassSlots,
        int economySlots,
        float businessClassPrice,
        float economyPrice,
        int to,
        int from)
    {
        Id = id;
        Code = code;
        Departure = departure;
        EstimatedArrival = estimatedArrival;
        BusinessClassSlots = businessClassSlots;
        EconomySlots = economySlots;
        BusinessClassPrice = businessClassPrice;
        EconomyPrice = economyPrice;
        To = to;
        From = from;
    }

    /// <summary>
    /// Obtient ou définit l'identifiant unique du vol.
    /// Pour une création, cette valeur peut être à 0.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Obtient ou définit le code unique du vol.
    /// Exemple : MD051, AF1234.
    /// </summary>
    [Required(ErrorMessage = "Le code du vol est requis.")]
    [MaxLength(30, ErrorMessage = "Le code du vol ne peut pas dépasser 30 caractères.")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Obtient ou définit la date et l'heure de départ prévues du vol.
    /// </summary>
    [Required(ErrorMessage = "La date de départ est requise.")]
    public DateTime Departure { get; set; }

    /// <summary>
    /// Obtient ou définit la date et l'heure estimée d'arrivée du vol.
    /// </summary>
    [Required(ErrorMessage = "La date d'arrivée estimée est requise.")]
    public DateTime EstimatedArrival { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre de places disponibles en classe affaires.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Le nombre de places en classe affaires doit être positif ou nul.")]
    public int BusinessClassSlots { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre de places disponibles en classe économique.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Le nombre de places en classe économique doit être positif ou nul.")]
    public int EconomySlots { get; set; }

    /// <summary>
    /// Obtient ou définit le tarif appliqué à la classe affaires.
    /// </summary>
    [Range(0, float.MaxValue, ErrorMessage = "Le tarif de la classe affaires doit être positif ou nul.")]
    public float BusinessClassPrice { get; set; }

    /// <summary>
    /// Obtient ou définit le tarif appliqué à la classe économique.
    /// </summary>
    [Range(0, float.MaxValue, ErrorMessage = "Le tarif de la classe économique doit être positif ou nul.")]
    public float EconomyPrice { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'aéroport de destination.
    /// </summary>
    [Required(ErrorMessage = "L'aéroport de destination est requis.")]
    public int To { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'aéroport de départ.
    /// </summary>
    [Required(ErrorMessage = "L'aéroport de départ est requis.")]
    public int From { get; set; }
}