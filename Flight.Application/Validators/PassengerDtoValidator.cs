using Flight.Domain.Entities;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour le DTO de passager.
/// </summary>
public class PassengerDtoValidator : AbstractValidator<PassengerDto>
{
    public PassengerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le prénom est requis.")
            .MaximumLength(50).WithMessage("Le prénom ne peut pas dépasser 50 caractères.")
            .Matches(@"^[a-zA-ZÀ-ÿ\s'-]+$").WithMessage("Le prénom ne doit contenir que des lettres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Le nom de famille est requis.")
            .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères.")
            .Matches(@"^[a-zA-ZÀ-ÿ\s'-]+$").WithMessage("Le nom ne doit contenir que des lettres.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Le deuxième prénom ne peut pas dépasser 100 caractères.")
            .When(x => !string.IsNullOrEmpty(x.MiddleName));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'adresse e-mail est requise.")
            .EmailAddress().WithMessage("L'adresse e-mail est invalide.");

        RuleFor(x => x.Contact)
            .NotEmpty().WithMessage("Le numéro de contact est requis.")
            .Matches(@"^\+?[0-9\s\-().]{7,20}$").WithMessage("Le numéro de contact est invalide.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("L'adresse est requise.")
            .MaximumLength(200).WithMessage("L'adresse ne peut pas dépasser 200 caractères.");

        RuleFor(x => x.Sex)
            .IsInEnum().WithMessage("Le genre spécifié est invalide.");
    }
}
