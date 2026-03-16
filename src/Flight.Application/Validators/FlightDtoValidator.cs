/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour l'objet <see cref="FlightDto"/>.
/// </summary>
public class FlightDtoValidator : AbstractValidator<FlightDto>
{
    /// <summary>
    /// Initialise les règles de validation pour <see cref="FlightDto"/>.
    /// </summary>
    public FlightDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Le code du vol est requis.")
            .MaximumLength(30)
            .WithMessage("Le code du vol ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.Departure)
            .NotEmpty()
            .WithMessage("La date de départ est requise.");

        RuleFor(x => x.EstimatedArrival)
            .NotEmpty()
            .WithMessage("La date d'arrivée estimée est requise.")
            .GreaterThan(x => x.Departure)
            .WithMessage("La date d'arrivée estimée doit être postérieure à la date de départ.");

        RuleFor(x => x.BusinessClassSlots)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Le nombre de places en classe affaires doit être supérieur ou égal à 0.");

        RuleFor(x => x.EconomySlots)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Le nombre de places en classe économique doit être supérieur ou égal à 0.");

        RuleFor(x => x.BusinessClassPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Le tarif de la classe affaires doit être supérieur ou égal à 0.");

        RuleFor(x => x.EconomyPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Le tarif de la classe économique doit être supérieur ou égal à 0.");

        RuleFor(x => x.To)
            .GreaterThan(0)
            .WithMessage("L'identifiant de l'aéroport de destination est requis.");

        RuleFor(x => x.From)
            .GreaterThan(0)
            .WithMessage("L'identifiant de l'aéroport de départ est requis.");

        RuleFor(x => x.From)
            .Must((dto, from) => from != dto.To)
            .WithMessage("L'aéroport de départ et l'aéroport de destination doivent être différents.");
    }
}