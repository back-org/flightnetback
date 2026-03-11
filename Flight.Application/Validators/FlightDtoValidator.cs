using Flight.Domain.Entities;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour le DTO de vol.
/// </summary>
public class FlightDtoValidator : AbstractValidator<FlightDto>
{
    public FlightDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Le code du vol est requis.")
            .MaximumLength(30).WithMessage("Le code du vol ne peut pas dépasser 30 caractères.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Le code du vol doit contenir uniquement des lettres majuscules et des chiffres.");

        RuleFor(x => x.Departure)
            .NotEmpty().WithMessage("La date de départ est requise.")
            .GreaterThan(DateTime.UtcNow.AddMinutes(-5))
            .WithMessage("La date de départ doit être dans le futur.");

        RuleFor(x => x.EstimatedArrival)
            .NotEmpty().WithMessage("La date d'arrivée estimée est requise.")
            .GreaterThan(x => x.Departure)
            .WithMessage("La date d'arrivée doit être postérieure à la date de départ.");

        RuleFor(x => x.BusinessClassSlots)
            .GreaterThanOrEqualTo(0).WithMessage("Le nombre de sièges affaires ne peut pas être négatif.");

        RuleFor(x => x.EconomySlots)
            .GreaterThanOrEqualTo(0).WithMessage("Le nombre de sièges économiques ne peut pas être négatif.");

        RuleFor(x => x.BusinessClassPrice)
            .GreaterThan(0).WithMessage("Le prix de la classe affaires doit être supérieur à 0.");

        RuleFor(x => x.EconomyPrice)
            .GreaterThan(0).WithMessage("Le prix de la classe économique doit être supérieur à 0.");

        RuleFor(x => x.To)
            .GreaterThan(0).WithMessage("L'identifiant de l'aéroport de destination est requis.");

        RuleFor(x => x.From)
            .GreaterThan(0).WithMessage("L'identifiant de l'aéroport de départ est requis.")
            .NotEqual(x => x.To).WithMessage("L'aéroport de départ et de destination ne peuvent pas être identiques.");
    }
}
