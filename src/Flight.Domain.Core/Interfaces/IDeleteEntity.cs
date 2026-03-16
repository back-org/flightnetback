/*
 * Rôle métier du fichier: Définir les contrats métier et techniques entre couches.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain.Core/Interfaces' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿namespace Flight.Domain.Core.Interfaces;

/// <summary>
///     The delete entity.
/// </summary>
public interface IDeleteEntity
{
    /// <summary>
    ///     Gets or sets a value indicating whether is deleted.
    /// </summary>
    bool IsDeleted { get; set; }
}

/// <summary>
///     The delete entity.
/// </summary>
public interface IDeleteEntity<TKey> : IDeleteEntity, IEntityBase<TKey>
{
}