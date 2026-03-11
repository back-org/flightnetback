using Flight.Domain.Entities;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour le DTO de compagnie aérienne.
/// </summary>
public class AirlineDtoValidator : AbstractValidator<AirlineDto>
{
    public AirlineDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom de la compagnie aérienne est requis.")
            .MaximumLength(30).WithMessage("Le nom ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.State)
            .IsInEnum().WithMessage("L'état de la compagnie est invalide.");
    }
}
