using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour l'objet <see cref="CityDto"/>.
/// </summary>
public class CityDtoValidator : AbstractValidator<CityDto>
{
    /// <summary>
    /// Initialise les règles de validation pour <see cref="CityDto"/>.
    /// </summary>
    public CityDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Le nom de la ville est requis.")
            .MaximumLength(30)
            .WithMessage("Le nom de la ville ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("L'identifiant du pays est requis.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("La latitude doit être comprise entre -90 et 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("La longitude doit être comprise entre -180 et 180.");
    }
}