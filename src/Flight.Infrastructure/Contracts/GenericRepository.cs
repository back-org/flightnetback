/*
 * Rôle métier du fichier: Fournir les implémentations techniques des services et dépôts métier.
 * Description: Ce fichier participe au sous-domaine 'Flight.Infrastructure/Contracts' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using System.Linq.Expressions;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Flight.Infrastructure.Contracts;

/// <summary>
/// Implémentation générique d'un repository basé sur Entity Framework Core.
/// 
/// Cette classe centralise les opérations CRUD les plus courantes
/// afin d'éviter de réécrire le même code pour chaque entité métier.
/// </summary>
/// <typeparam name="T">Type d'entité géré par le repository.</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    /// <summary>
    /// Contexte EF Core utilisé pour accéder à la base de données.
    /// </summary>
    protected readonly FlightContext Context;

    /// <summary>
    /// Ensemble EF Core représentant la table liée à l'entité.
    /// </summary>
    protected readonly DbSet<T> DbSet;

    /// <summary>
    /// Initialise une nouvelle instance du repository générique.
    /// </summary>
    /// <param name="context">Contexte de base de données injecté par DI.</param>
    public GenericRepository(FlightContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }

    /// <inheritdoc />
    public virtual async Task<IEnumerable<T>> AllAsync()
    {
        return await DbSet
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await DbSet
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync();
    }

    /// <inheritdoc />
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    /// <inheritdoc />
    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate);
    }

    /// <inheritdoc />
    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await DbSet.AnyAsync(predicate);
    }

    /// <inheritdoc />
    public virtual async Task<int> AddAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await DbSet.AddAsync(entity);
        return await Context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public virtual async Task<int> AddRangeAsync(IEnumerable<T> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await DbSet.AddRangeAsync(entities);
        return await Context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public virtual async Task<int> Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbSet.Update(entity);
        return await Context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public virtual async Task<int> Delete(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        DbSet.Remove(entity);
        return await Context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public virtual async Task<int> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null)
        {
            return 0;
        }

        DbSet.Remove(entity);
        return await Context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public virtual async Task<int> CountAsync()
    {
        return await DbSet.CountAsync();
    }

    /// <inheritdoc />
    public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return await DbSet.CountAsync(predicate);
    }
}