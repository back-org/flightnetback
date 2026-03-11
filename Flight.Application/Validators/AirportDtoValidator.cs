using Flight.Domain.Entities;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour le DTO d'aéroport.
/// </summary>
public class AirportDtoValidator : AbstractValidator<AirportDto>
{
    public AirportDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom de l'aéroport est requis.")
            .MaximumLength(30).WithMessage("Le nom ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.State)
            .IsInEnum().WithMessage("L'état opérationnel de l'aéroport est invalide.");
    }
}
