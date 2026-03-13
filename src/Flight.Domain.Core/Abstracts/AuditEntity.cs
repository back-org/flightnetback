using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Flight.Domain.Core.Attributes;
using Flight.Domain.Core.Interfaces;

namespace Flight.Domain.Core.Abstracts;

/// <summary>
///     The audit entity.
/// </summary>
public abstract class AuditEntity<TKey> : DeleteEntity<TKey>, IAuditEntity<TKey>
{
    /// <summary>
    ///     Gets or sets the created date.
    /// </summary>
    [Column("created")]
    [DataType(DataType.Date)]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    ///     Gets or sets the created by.
    /// </summary>
    [Column("created_by")]
    public string CreatedBy { get; set; }

    /// <summary>
    ///     Gets or sets the updated date.
    /// </summary>
    [UpdateGreaterThanCreate("CreatedDate")]
    [Column("updated")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedDate { get; set; }

    /// <summary>
    ///     Gets or sets the updated by.
    /// </summary>
    [Column("updated_by")]
    public string UpdatedBy { get; set; }
}