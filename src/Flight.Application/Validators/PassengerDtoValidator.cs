using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour l'objet <see cref="PassengerDto"/>.
/// </summary>
public class PassengerDtoValidator : AbstractValidator<PassengerDto>
{
    /// <summary>
    /// Initialise les règles de validation pour <see cref="PassengerDto"/>.
    /// </summary>
    public PassengerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Le prénom du passager est requis.")
            .MaximumLength(50)
            .WithMessage("Le prénom du passager ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(100)
            .WithMessage("Le deuxième prénom ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Le nom de famille du passager est requis.")
            .MaximumLength(100)
            .WithMessage("Le nom de famille du passager ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("L'adresse e-mail du passager est requise.")
            .EmailAddress()
            .WithMessage("L'adresse e-mail du passager est invalide.");

        RuleFor(x => x.Contact)
            .NotEmpty()
            .WithMessage("Le contact du passager est requis.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("L'adresse du passager est requise.");

        RuleFor(x => x.Sex)
            .IsInEnum()
            .WithMessage("Le genre du passager est invalide.");
    }
}