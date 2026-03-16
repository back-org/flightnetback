/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using System.ComponentModel.DataAnnotations;

namespace Flight.Application.DTOs;

/// <summary>
/// DTO représentant un pays.
/// Utilisé pour les opérations CRUD via l'API.
/// </summary>
public class CountryDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="CountryDto"/>.
    /// </summary>
    public CountryDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="CountryDto"/>.
    /// </summary>
    public CountryDto(
        int id,
        string name,
        string iso2,
        string iso3)
    {
        Id = id;
        Name = name;
        Iso2 = iso2;
        Iso3 = iso3;
    }

    /// <summary>
    /// Identifiant unique du pays.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom officiel du pays.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Code ISO 3166-1 alpha-2 du pays.
    /// </summary>
    [Required]
    [StringLength(2)]
    public string Iso2 { get; set; } = string.Empty;

    /// <summary>
    /// Code ISO 3166-1 alpha-3 du pays.
    /// </summary>
    [Required]
    [StringLength(3)]
    public string Iso3 { get; set; } = string.Empty;
}