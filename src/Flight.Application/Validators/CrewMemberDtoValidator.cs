using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO membre d'équipe.
/// Vérifie que les informations professionnelles essentielles sont bien présentes.
/// </summary>
public class CrewMemberDtoValidator : AbstractValidator<CrewMemberDto>
{
    /// <summary>
    /// Initialise les règles de validation du DTO membre d'équipe.
    /// </summary>
    public CrewMemberDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("L'identifiant utilisateur doit être supérieur à zéro.");

        RuleFor(x => x.EmployeeNumber)
            .NotEmpty().WithMessage("Le numéro matricule est requis.")
            .MaximumLength(30).WithMessage("Le numéro matricule ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("La fonction est requise.")
            .MaximumLength(50).WithMessage("La fonction ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.LicenseNumber)
            .MaximumLength(50).WithMessage("Le numéro de licence ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Le statut est requis.")
            .MaximumLength(30).WithMessage("Le statut ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("La date d'embauche ne peut pas être dans le futur.");
    }
}