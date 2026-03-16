/*
 * Rôle métier du fichier: Structurer les modèles de réponse et de pagination de l’API.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain.Core/Models' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿namespace Flight.Domain.Core.Models;

/// <summary>
///     The base model.
/// </summary>
public class BaseModel
{
    /// <summary>
    ///     Gets or sets the id.
    /// </summary>
    public int Id { get; set; }
}