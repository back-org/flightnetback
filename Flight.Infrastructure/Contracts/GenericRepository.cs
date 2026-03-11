using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Flight.Infrastructure.Contracts;

/// <summary>
/// Implémentation générique du repository basée sur Entity Framework Core.
/// </summary>
/// <typeparam name="T">Type de l'entité manipulée.</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    /// <summary>
    /// Contexte de base de données de l'application.
    /// </summary>
    protected readonly FlightContext Context;

    /// <summary>
    /// Jeu d'entités EF Core.
    /// </summary>
    protected readonly DbSet<T> DbSet;

    /// <summary>
    /// Initialise une nouvelle instance du repository générique.
    /// </summary>
    /// <param name="context">Le contexte EF Core injecté.</param>
    public GenericRepository(FlightContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    /// <summary>
    /// Retourne tous les éléments.
    /// </summary>
    /// <returns>Une collection de type <typeparamref name="T"/>.</returns>
    public async Task<IEnumerable<T>> AllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Construit une requête filtrée.
    /// </summary>
    /// <param name="predicate">Le filtre à appliquer.</param>
    /// <param name="trackChanges">Indique si EF doit suivre les modifications.</param>
    /// <returns>Une requête IQueryable.</returns>
    public IQueryable<T> Get(Expression<Func<T, bool>> predicate, bool trackChanges = false)
    {
        return trackChanges
            ? DbSet.Where(predicate)
            : DbSet.AsNoTracking().Where(predicate);
    }

    /// <summary>
    /// Retourne une entité par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant recherché.</param>
    /// <returns>L'entité si elle existe, sinon null.</returns>
    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    /// <summary>
    /// Ajoute une nouvelle entité et persiste les changements.
    /// </summary>
    /// <param name="entity">L'entité à ajouter.</param>
    /// <returns>Le nombre de lignes affectées.</returns>
    public async Task<int> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return await Save();
    }

    /// <summary>
    /// Met à jour une entité et persiste les changements.
    /// </summary>
    /// <param name="entity">L'entité à mettre à jour.</param>
    /// <returns>Le nombre de lignes affectées.</returns>
    public async Task<int> Update(T entity)
    {
        DbSet.Update(entity);
        return await Save();
    }

    /// <summary>
    /// Supprime une entité et persiste les changements.
    /// </summary>
    /// <param name="entity">L'entité à supprimer.</param>
    /// <returns>Le nombre de lignes affectées.</returns>
    public async Task<int> Delete(T entity)
    {
        DbSet.Remove(entity);
        return await Save();
    }

    /// <summary>
    /// Supprime une entité à partir de son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'entité.</param>
    /// <returns>Le nombre de lignes affectées.</returns>
    public async Task<int> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null)
        {
            return 0;
        }

        DbSet.Remove(entity);
        return await Save();
    }

    /// <summary>
    /// Sauvegarde les changements sur la base.
    /// </summary>
    /// <returns>Le nombre de lignes affectées.</returns>
    public async Task<int> Save()
    {
        return await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Retourne une liste paginée d'éléments.
    /// </summary>
    /// <param name="pageNumber">Numéro de page.</param>
    /// <param name="pageSize">Taille de page.</param>
    /// <returns>Une liste paginée.</returns>
    public async Task<IReadOnlyList<T>> SelectAllByPageAsync(int pageNumber, int pageSize)
    {
        var skip = (pageNumber - 1) * pageSize;

        return await DbSet
            .AsNoTracking()
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Retourne le nombre total d'enregistrements.
    /// </summary>
    /// <returns>Le total des enregistrements.</returns>
    public async Task<int> CountAsync()
    {
        return await DbSet.CountAsync();
    }
}