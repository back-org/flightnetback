/*
 * Rôle métier du fichier: Structurer les modèles de réponse et de pagination de l’API.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Models' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

namespace Flight.Api.Models;

/// <summary>
/// Représente un résultat paginé retourné par l'API.
/// </summary>
/// <typeparam name="T">Le type des éléments paginés.</typeparam>
public sealed class PagedResult<T>
{
    /// <summary>
    /// Liste paginée des éléments.
    /// </summary>
    public IReadOnlyCollection<T> Items { get; init; } = [];

    /// <summary>
    /// Numéro de page courant.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Taille de page courante.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Nombre total d'éléments disponibles.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Nombre total de pages.
    /// </summary>
    public int TotalPages =>
        PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}