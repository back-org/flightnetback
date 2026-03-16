/*
 * Rôle métier du fichier: Structurer les modèles de réponse et de pagination de l’API.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Models' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

namespace Flight.Api.Models;

/// <summary>
/// Représente les paramètres standards de pagination d'une requête.
/// </summary>
public sealed class PagedQuery
{
    private const int MaxPageSize = 100;

    private int _pageNumber = 1;
    private int _pageSize = 20;

    /// <summary>
    /// Numéro de page demandé.
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Taille de page demandée.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value switch
        {
            < 1 => 20,
            > MaxPageSize => MaxPageSize,
            _ => value
        };
    }
}