/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant une ville.
/// Utilisé pour la gestion des villes dans l'API.
/// </summary>
public class CityDto
{
    /// <summary>
    /// Constructeur vide requis pour la sérialisation.
    /// </summary>
    public CityDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="CityDto"/>.
    /// </summary>
    public CityDto(
        int id,
        string name,
        int countryId,
        double latitude,
        double longitude)
    {
        Id = id;
        Name = name;
        CountryId = countryId;
        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Identifiant unique de la ville.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom de la ville.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Identifiant du pays auquel appartient la ville.
    /// </summary>
    [Required]
    public int CountryId { get; set; }

    /// <summary>
    /// Latitude géographique de la ville.
    /// </summary>
    [Range(-90, 90)]
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude géographique de la ville.
    /// </summary>
    [Range(-180, 180)]
    public double Longitude { get; set; }
}