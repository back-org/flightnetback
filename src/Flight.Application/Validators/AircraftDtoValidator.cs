using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO avion.
/// Vérifie que les informations de flotte sont cohérentes.
/// </summary>
public class AircraftDtoValidator : AbstractValidator<AircraftDto>
{
    /// <summary>
    /// Initialise les règles de validation du DTO avion.
    /// </summary>
    public AircraftDtoValidator()
    {
        RuleFor(x => x.RegistrationNumber)
            .NotEmpty().WithMessage("Le numéro d'immatriculation est requis.")
            .MaximumLength(30).WithMessage("Le numéro d'immatriculation ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.Manufacturer)
            .MaximumLength(50).WithMessage("Le fabricant ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.Model)
            .MaximumLength(50).WithMessage("Le modèle ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.BusinessCapacity)
            .GreaterThanOrEqualTo(0).WithMessage("La capacité business doit être positive ou nulle.");

        RuleFor(x => x.EconomyCapacity)
            .GreaterThanOrEqualTo(0).WithMessage("La capacité economy doit être positive ou nulle.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Le statut de l'avion est requis.")
            .MaximumLength(30).WithMessage("Le statut de l'avion ne peut pas dépasser 30 caractères.");

        RuleFor(x => x)
            .Must(x => x.BusinessCapacity + x.EconomyCapacity > 0)
            .WithMessage("L'avion doit avoir au moins une place disponible au total.");

        RuleFor(x => x.LastMaintenanceDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.LastMaintenanceDate.HasValue)
            .WithMessage("La date de maintenance ne peut pas être dans le futur.");
    }
}