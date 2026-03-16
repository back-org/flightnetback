/*
 * Rôle métier du fichier: Définir les contrats métier et techniques entre couches.
 * Description: Ce fichier participe au sous-domaine 'Flight.Domain/Interfaces' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using System.Linq.Expressions;

namespace Flight.Domain.Interfaces;

/// <summary>
/// Représente un repository générique permettant d'effectuer
/// les opérations CRUD de base sur une entité.
/// 
/// Cette interface évite de réécrire les mêmes méthodes
/// pour chaque type d'entité.
/// </summary>
/// <typeparam name="T">Type de l'entité gérée.</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Retourne tous les éléments de l'entité.
    /// </summary>
    /// <returns>Liste complète des éléments.</returns>
    Task<IEnumerable<T>> AllAsync();

    /// <summary>
    /// Retourne tous les éléments correspondant à un filtre.
    /// </summary>
    /// <param name="predicate">Condition de filtrage.</param>
    /// <returns>Liste filtrée des éléments.</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Retourne un élément à partir de son identifiant entier.
    /// </summary>
    /// <param name="id">Identifiant de l'entité.</param>
    /// <returns>L'entité trouvée, ou null si absente.</returns>
    Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Retourne le premier élément correspondant à un filtre.
    /// </summary>
    /// <param name="predicate">Condition de recherche.</param>
    /// <returns>L'entité trouvée, ou null si absente.</returns>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Indique si au moins un élément correspond à la condition donnée.
    /// </summary>
    /// <param name="predicate">Condition à vérifier.</param>
    /// <returns>True si au moins un élément existe, sinon false.</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Ajoute une nouvelle entité à la base de données.
    /// </summary>
    /// <param name="entity">Entité à ajouter.</param>
    /// <returns>Nombre de lignes impactées.</returns>
    Task<int> AddAsync(T entity);

    /// <summary>
    /// Ajoute plusieurs entités en une seule opération.
    /// </summary>
    /// <param name="entities">Liste des entités à ajouter.</param>
    /// <returns>Nombre de lignes impactées.</returns>
    Task<int> AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// Met à jour une entité existante.
    /// </summary>
    /// <param name="entity">Entité à mettre à jour.</param>
    /// <returns>Nombre de lignes impactées.</returns>
    Task<int> Update(T entity);

    /// <summary>
    /// Supprime une entité existante.
    /// </summary>
    /// <param name="entity">Entité à supprimer.</param>
    /// <returns>Nombre de lignes impactées.</returns>
    Task<int> Delete(T entity);

    /// <summary>
    /// Supprime une entité à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant de l'entité à supprimer.</param>
    /// <returns>Nombre de lignes impactées.</returns>
    Task<int> DeleteAsync(int id);

    /// <summary>
    /// Retourne le nombre total d'éléments présents.
    /// </summary>
    /// <returns>Nombre total d'entités.</returns>
    Task<int> CountAsync();

    /// <summary>
    /// Retourne le nombre d'éléments correspondant à un filtre.
    /// </summary>
    /// <param name="predicate">Condition de filtrage.</param>
    /// <returns>Nombre d'éléments trouvés.</returns>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
}