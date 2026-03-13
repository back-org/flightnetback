using Flight.Domain.Core.Abstracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flight.Infrastructure.Interfaces;

/// <summary>
///     The generic application.
/// </summary>
public interface IService<T> where T : DeleteEntity<int>
{
    /// <summary>
    /// Gets the all async.
    /// </summary>
    /// <param name="trackChanges">If true, track changes.</param>
    /// <returns>A Task.</returns>
    Task<IEnumerable<DeleteEntity<int>>> GetAllAsync(bool trackChanges);

    /// <summary>
    /// Gets the async.
    /// </summary>
    /// <param name="Id">The id.</param>
    /// <param name="trackChanges">If true, track changes.</param>
    /// <returns>A Task.</returns>
    Task<DeleteEntity<int>> GetAsync(int Id, bool trackChanges);

    /// <summary>
    /// Creates the async.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A Task.</returns>
    Task<int> CreateAsync(DeleteEntity<int> entity);

    /// <summary>
    /// Gets the by ids async.
    /// </summary>
    /// <param name="ids">The ids.</param>
    /// <param name="trackChanges">If true, track changes.</param>
    /// <returns>A Task.</returns>
    Task<IEnumerable<DeleteEntity<int>>> GetByIdsAsync(IEnumerable<int> ids, bool
        trackChanges);

    /// <summary>
    /// Creates the collection async.
    /// </summary>
    /// <param name="entityCollection">The entity collection.</param>
    /// <returns>A Task.</returns>
    Task<(IEnumerable<DeleteEntity<int>> entities, string ids)> CreateCollectionAsync
        (IEnumerable<DeleteEntity<int>> entityCollection);

    /// <summary>
    /// Deletes the async.
    /// </summary>
    /// <param name="Id">The id.</param>
    /// <param name="trackChanges">If true, track changes.</param>
    /// <returns>A Task.</returns>
    Task DeleteAsync(int Id, bool trackChanges);

    /// <summary>
    /// Updates the async.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="entityForUpdate">The entity for update.</param>
    /// <param name="trackChanges">If true, track changes.</param>
    /// <returns>A Task.</returns>
    Task UpdateAsync(int id, DeleteEntity<int> entityForUpdate, bool trackChanges);
}