using System;
using System.ComponentModel.DataAnnotations;

namespace Flight.Domain.Core.Attributes;

/// <summary>
///     The date greater than attribute. Validate the date comparison
/// </summary>
public class UpdateGreaterThanCreateAttribute(string comparisonProperty) : ValidationAttribute
{
    private readonly string _comparisonProperty = comparisonProperty;

    /// <summary>
    ///     Are the valid.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="validationContext">The validation context.</param>
    /// <returns>A ValidationResult.</returns>
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null) return ValidationResult.Success;
        var currentValue = (DateTime)value;

        var comparisonValue = (DateTime)validationContext.ObjectType.GetProperty(_comparisonProperty)
            ?.GetValue(validationContext.ObjectInstance)!;

        return currentValue < comparisonValue
            ? new ValidationResult(ErrorMessage = "The update date must be later than start date")
            : ValidationResult.Success;
    }
}