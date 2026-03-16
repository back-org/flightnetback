/*
 * Rôle métier du fichier: Définir les contrats métier et techniques entre couches.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain.Core/Interfaces' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using System;

namespace Flight.Domain.Core.Interfaces;

/// <summary>
///     The audit entity.
/// </summary>
public interface IAuditEntity
{
    /// <summary>
    ///     Gets or sets the created date.
    /// </summary>
    DateTime CreatedDate { get; set; }

    /// <summary>
    ///     Gets or sets the created by.
    /// </summary>
    string CreatedBy { get; set; }

    /// <summary>
    ///     Gets or sets the updated date.
    /// </summary>
    DateTime? UpdatedDate { get; set; }

    /// <summary>
    ///     Gets or sets the updated by.
    /// </summary>
    string UpdatedBy { get; set; }
}

/// <summary>
///     The audit entity.
/// </summary>
public interface IAuditEntity<TKey> : IAuditEntity, IDeleteEntity<TKey>
{
}