using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Interfaces;

namespace Flight.Domain.Core.Abstracts;

/// <summary>
///     The delete entity.
/// </summary>
public abstract class DeleteEntity<TKey> : EntityBase<TKey>, IDeleteEntity<TKey>
{
    /// <summary>
    ///     Gets or sets a value indicating whether is active.
    /// </summary>
    [Column("activated")]
    public bool IsActive { get; set; } = true;

    /// <summary>
    ///     Gets or sets a value indicating whether is deleted.
    /// </summary>
    [Column("deleted")]
    public bool IsDeleted { get; set; } = false;
}