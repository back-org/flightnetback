namespace Flight.Domain.Core.Interfaces;

/// <summary>
///     The delete entity.
/// </summary>
public interface IDeleteEntity
{
    /// <summary>
    ///     Gets or sets a value indicating whether is deleted.
    /// </summary>
    bool IsDeleted { get; set; }
}

/// <summary>
///     The delete entity.
/// </summary>
public interface IDeleteEntity<TKey> : IDeleteEntity, IEntityBase<TKey>
{
}