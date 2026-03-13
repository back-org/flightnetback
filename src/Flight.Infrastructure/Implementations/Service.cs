using Flight.Domain.Core.Abstracts;
using Flight.Domain.Entities;
using Flight.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flight.Infrastructure.Implementations;

/// <summary>
/// The service.
/// </summary>
public abstract class Service<T>(IRepositoryManager repository, ILoggerManager logger) : IService<T>
    where T : DeleteEntity<int>
{
    protected readonly IRepositoryManager Repository = repository;
    protected readonly ILoggerManager Logger = logger;

    /// <summary>
    /// Creates the async.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A Task.</returns>
    public async Task<int> CreateAsync(DeleteEntity<int> entity)
    {
        if (entity is Airline airline) return await Repository.Airline.AddAsync(airline);

        return 0;
    }

    public Task<(IEnumerable<DeleteEntity<int>> entities, string ids)> CreateCollectionAsync(
        IEnumerable<DeleteEntity<int>> entityCollection)
    {
        throw new System.NotImplementedException();
    }

    public Task DeleteAsync(int id, bool trackChanges)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<DeleteEntity<int>>> GetAllAsync(bool trackChanges)
    {
        throw new System.NotImplementedException();
    }

    public Task<DeleteEntity<int>> GetAsync(int id, bool trackChanges)
    {
        throw new System.NotImplementedException();
    }

    public Task<IEnumerable<DeleteEntity<int>>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateAsync(int id, DeleteEntity<int> entityForUpdate, bool trackChanges)
    {
        throw new System.NotImplementedException();
    }
}