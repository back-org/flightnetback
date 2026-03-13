using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour l'objet <see cref="VehicleDto"/>.
/// </summary>
public class VehicleDtoValidator : AbstractValidator<VehicleDto>
{
    /// <summary>
    /// Initialise les règles de validation pour <see cref="VehicleDto"/>.
    /// </summary>
    public VehicleDtoValidator()
    {
        RuleFor(x => x.LicensePlate)
            .NotEmpty()
            .WithMessage("La plaque d'immatriculation est requise.")
            .MaximumLength(20)
            .WithMessage("La plaque d'immatriculation ne peut pas dépasser 20 caractères.");

        RuleFor(x => x.Manufacturer)
            .NotEmpty()
            .WithMessage("Le fabricant du véhicule est requis.");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Le modèle du véhicule est requis.");

        RuleFor(x => x.Year)
            .InclusiveBetween((short)1900, (short)2100)
            .WithMessage("L'année du véhicule doit être comprise entre 1900 et 2100.");

        RuleFor(x => x.Tariff)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Le tarif du véhicule doit être supérieur ou égal à 0.");
    }
}