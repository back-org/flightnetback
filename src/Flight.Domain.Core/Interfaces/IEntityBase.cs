/*
 * Rôle métier du fichier: Définir les contrats métier et techniques entre couches.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain.Core/Interfaces' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿namespace Flight.Domain.Core.Interfaces;

/// <summary>
///     The entity base.
/// </summary>
public interface IEntityBase<TKey>
{
    /// <summary>
    ///     Gets or sets the id.
    /// </summary>
    TKey Id { get; set; }
}