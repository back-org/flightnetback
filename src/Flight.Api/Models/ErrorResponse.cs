/*
 * Rôle métier du fichier: Structurer les modèles de réponse et de pagination de l’API.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Models' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿namespace Flight.Api.Models;

/// <summary>
/// Représente un format standard d'erreur retourné par l'API.
/// </summary>
public sealed class ErrorResponse
{
    /// <summary>
    /// Code HTTP associé à l'erreur.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Message principal décrivant l'erreur.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Détail complémentaire utile au diagnostic.
    /// </summary>
    public string? Detail { get; set; }

    /// <summary>
    /// Identifiant de traçabilité de la requête.
    /// </summary>
    public string? TraceId { get; set; }
}