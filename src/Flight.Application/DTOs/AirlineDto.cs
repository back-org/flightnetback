/*
 * Rôle métier du fichier: Transporter les données métier entre couches sans exposer les entités internes.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/DTOs' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using System.ComponentModel.DataAnnotations;
using Flight.Domain.Entities;

namespace Flight.Application.DTOs;

/// <summary>
/// Objet de transfert de données (DTO) représentant une compagnie aérienne.
/// Utilisé par l'API pour les opérations de lecture, création et mise à jour.
/// </summary>
public class AirlineDto
{
    /// <summary>
    /// Initialise une nouvelle instance vide de <see cref="AirlineDto"/>.
    /// Requis notamment pour la sérialisation / désérialisation JSON.
    /// </summary>
    public AirlineDto()
    {
    }

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="AirlineDto"/> avec les valeurs fournies.
    /// </summary>
    /// <param name="id">Identifiant unique de la compagnie aérienne.</param>
    /// <param name="name">Nom officiel de la compagnie aérienne.</param>
    /// <param name="state">État d'activité de la compagnie.</param>
    /// <param name="deletedFlag">Indicateur de suppression logique.</param>
    public AirlineDto(int id, string name, State state, int deletedFlag)
    {
        Id = id;
        Name = name;
        State = state;
        DeletedFlag = deletedFlag;
    }

    /// <summary>
    /// Identifiant unique de la compagnie aérienne.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom officiel de la compagnie aérienne.
    /// </summary>
    [Required(ErrorMessage = "Le nom de la compagnie aérienne est requis.")]
    [MaxLength(30, ErrorMessage = "Le nom ne peut pas dépasser 30 caractères.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// État d'activité de la compagnie aérienne.
    /// </summary>
    public State State { get; set; } = State.Active;

    /// <summary>
    /// Indicateur de suppression logique.
    /// Une valeur non nulle signale une suppression douce.
    /// </summary>
    public int DeletedFlag { get; set; }
}