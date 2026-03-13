using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Flight.Domain.Interfaces;

/// <summary>
/// Contrat générique pour les opérations CRUD et les requêtes standard.
/// </summary>
/// <typeparam name="T">Type de l'entité manipulée.</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Retourne tous les éléments de l'entité.
    /// </summary>
    /// <returns>Une collection de type <typeparamref name="T"/>.</returns>
    Task<IEnumerable<T>> AllAsync();

    /// <summary>
    /// Retourne les éléments correspondant à un prédicat.
    /// </summary>
    /// <param name="predicate">Le filtre appliqué.</param>
    /// <param name="trackChanges">Indique si EF Core doit tracker les entités.</param>
    /// <returns>Une requête IQueryable.</returns>
    IQueryable<T> Get(Expression<Func<T, bool>> predicate, bool trackChanges = false);

    /// <summary>
    /// Retourne une entité par identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'entité.</param>
    /// <returns>L'entité si trouvée, sinon null.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Ajoute une entité dans le contexte et sauvegarde.
    /// </summary>
    /// <param name="entity">L'entité à ajouter.</param>
    /// <returns>Le nombre de lignes affectées.</returns>
    Task<int> AddAsync(T entity);

    /// <summary>
    /// Met à jour une entité et sauvegarde.
    /// </summary>
    /// <param name="entity">L'entité à mettre à jour.</param>
    /// <returns>Le nombre de lignes affectées.</returns>
    Task<int> Update(T entity);

    /// <summary>
    /// Supprime une entité et sauvegarde.
    /// </summary>
    /// <param name="entity">L'entité à supprimer.</param>
    /// <returns>Le nombre de lignes affectées.</returns>
    Task<int> Delete(T entity);

    /// <summary>
    /// Supprime une entité par identifiant et sauvegarde.
    /// </summary>
    /// <param name="id">L'identifiant à supprimer.</param>
    /// <returns>Le nombre de lignes affectées.</returns>
    Task<int> DeleteAsync(int id);

    /// <summary>
    /// Sauvegarde explicitement les changements en base.
    /// </summary>
    /// <returns>Le nombre de lignes affectées.</returns>
    Task<int> Save();

    /// <summary>
    /// Retourne un sous-ensemble paginé des éléments.
    /// </summary>
    /// <param name="pageNumber">Numéro de page.</param>
    /// <param name="pageSize">Taille de page.</param>
    /// <returns>Une liste paginée.</returns>
    Task<IReadOnlyList<T>> SelectAllByPageAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Retourne le nombre total d'éléments.
    /// </summary>
    /// <returns>Le nombre total d'enregistrements.</returns>
    Task<int> CountAsync();
}