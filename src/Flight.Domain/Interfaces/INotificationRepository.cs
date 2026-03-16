/*
 * Rôle métier du fichier: Définir les contrats métier et techniques entre couches.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain/Interfaces' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Domain.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flight.Domain.Interfaces;

/// <summary>
/// Interface du dépôt de notifications générique avec pattern Result.
/// Fournit des méthodes CRUD asynchrones retournant des objets <see cref="Result"/> ou <see cref="Result{T}"/>.
/// </summary>
/// <typeparam name="T">Le type d'entité géré par ce dépôt.</typeparam>
public interface INotificationRepository<T> where T : class
{
    /// <summary>
    /// Retourne toutes les entités sous forme de requête IQueryable.
    /// </summary>
    /// <param name="trackChanges">
    /// Si <c>true</c>, Entity Framework suit les changements sur les entités retournées.
    /// Si <c>false</c>, les entités sont retournées en lecture seule (AsNoTracking).
    /// </param>
    /// <returns>Un <see cref="IQueryable{T}"/> contenant toutes les entités.</returns>
    IQueryable<T> FindAll(bool trackChanges);

    /// <summary>
    /// Met à jour une entité existante de manière asynchrone.
    /// </summary>
    /// <param name="old">L'entité originale à remplacer.</param>
    /// <param name="entity">L'entité avec les nouvelles valeurs.</param>
    /// <returns>Un <see cref="Result"/> indiquant le succès ou l'échec de l'opération.</returns>
    Task<Result> PutAsync(T old, T entity);

    /// <summary>
    /// Crée une nouvelle entité de manière asynchrone.
    /// </summary>
    /// <param name="entity">L'entité à créer.</param>
    /// <returns>Un <see cref="Result"/> indiquant le succès ou l'échec de l'opération.</returns>
    Task<Result> PostAsync(T entity);

    /// <summary>
    /// Récupère une entité par son identifiant de manière asynchrone.
    /// </summary>
    /// <param name="id">L'identifiant de l'entité à récupérer.</param>
    /// <returns>Un <see cref="Result{T}"/> contenant l'entité trouvée, ou une erreur si non trouvée.</returns>
    Task<Result<T>> SelectByIdAsync(int id);

    /// <summary>
    /// Récupère toutes les entités de manière asynchrone (via IQueryable).
    /// </summary>
    /// <returns>Un <see cref="Result{T}"/> contenant la collection d'entités.</returns>
    Task<Result<IEnumerable<T>>> GetAllAsync();

    /// <summary>
    /// Supprime une entité par son identifiant de manière asynchrone.
    /// </summary>
    /// <param name="id">L'identifiant de l'entité à supprimer.</param>
    /// <returns>Un <see cref="Result"/> indiquant le succès ou l'échec de l'opération.</returns>
    Task<Result> RemoveAsync(int id);

    /// <summary>
    /// Récupère toutes les entités de manière asynchrone (via liste en mémoire).
    /// </summary>
    /// <returns>Un <see cref="Result{T}"/> contenant la collection d'entités.</returns>
    Task<Result<IEnumerable<T>>> GetAsync();
}
