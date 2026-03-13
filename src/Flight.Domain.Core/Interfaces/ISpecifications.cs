using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Flight.Domain.Core.Interfaces;

/// <summary>
///     The specifications.
/// </summary>
public interface ISpecifications<T>
{
    /// <summary>
    ///     Gets the criteria.
    /// </summary>
    Expression<Func<T, bool>> Criteria { get; }

    /// <summary>
    ///     Gets the includes.
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    ///     Gets the order by.
    /// </summary>
    Expression<Func<T, object>> OrderBy { get; }

    /// <summary>
    ///     Gets the order by descending.
    /// </summary>
    Expression<Func<T, object>> OrderByDescending { get; }

    /// <summary>
    ///     Gets the take.
    /// </summary>
    int Take { get; }

    /// <summary>
    ///     Gets the skip.
    /// </summary>
    int Skip { get; }

    /// <summary>
    ///     Gets a value indicating whether paging is enabled.
    /// </summary>
    bool IsPagingEnabled { get; }
}