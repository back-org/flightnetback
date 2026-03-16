/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO bagage.
/// </summary>
public class BaggageDtoValidator : AbstractValidator<BaggageDto>
{
    public BaggageDtoValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0).WithMessage("L'identifiant de la réservation doit être supérieur à zéro.");

        RuleFor(x => x.PassengerId)
            .GreaterThan(0).WithMessage("L'identifiant du passager doit être supérieur à zéro.");

        RuleFor(x => x.FlightId)
            .GreaterThan(0).WithMessage("L'identifiant du vol doit être supérieur à zéro.");

        RuleFor(x => x.TagNumber)
            .MaximumLength(50).WithMessage("Le numéro d'étiquette ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.Weight)
            .GreaterThanOrEqualTo(0).WithMessage("Le poids du bagage doit être positif ou nul.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Le type de bagage est requis.")
            .MaximumLength(30).WithMessage("Le type ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Le statut du bagage est requis.")
            .MaximumLength(30).WithMessage("Le statut ne peut pas dépasser 30 caractères.");
    }
}