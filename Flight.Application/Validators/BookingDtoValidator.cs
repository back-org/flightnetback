using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour l'objet <see cref="BookingDto"/>.
/// </summary>
public class BookingDtoValidator : AbstractValidator<BookingDto>
{
    /// <summary>
    /// Initialise les règles de validation pour <see cref="BookingDto"/>.
    /// </summary>
    public BookingDtoValidator()
    {
        RuleFor(x => x.FlightId)
            .GreaterThan(0)
            .WithMessage("L'identifiant du vol doit être supérieur à 0.");

        RuleFor(x => x.PassengerId)
            .GreaterThan(0)
            .WithMessage("L'identifiant du passager doit être supérieur à 0.");

        RuleFor(x => x.FlightType)
            .IsInEnum()
            .WithMessage("La classe de confort spécifiée est invalide.");

        RuleFor(x => x.Statut)
            .IsInEnum()
            .WithMessage("Le statut de réservation spécifié est invalide.");
    }
}