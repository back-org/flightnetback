namespace Flight.Domain.Core.Interfaces;

/// <summary>
///     The entity base.
/// </summary>
public interface IEntityBase<TKey>
{
    /// <summary>
    ///     Gets or sets the id.
    /// </summary>
    TKey Id { get; set; }
}