using Flight.Domain.Entities;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour le DTO de réservation.
/// </summary>
public class BookingDtoValidator : AbstractValidator<BookingDto>
{
    public BookingDtoValidator()
    {
        RuleFor(x => x.FlightId)
            .GreaterThan(0).WithMessage("L'identifiant du vol est requis.");

        RuleFor(x => x.PassengerId)
            .GreaterThan(0).WithMessage("L'identifiant du passager est requis.");

        RuleFor(x => x.FlightType)
            .IsInEnum().WithMessage("La classe de confort spécifiée est invalide.");

        RuleFor(x => x.Statut)
            .IsInEnum().WithMessage("Le statut de réservation spécifié est invalide.");
    }
}
