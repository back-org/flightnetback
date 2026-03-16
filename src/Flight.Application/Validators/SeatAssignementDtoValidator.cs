/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO attribution de siège.
/// </summary>
public class SeatAssignmentDtoValidator : AbstractValidator<SeatAssignmentDto>
{
    public SeatAssignmentDtoValidator()
    {
        RuleFor(x => x.FlightId)
            .GreaterThan(0).WithMessage("L'identifiant du vol doit être supérieur à zéro.");

        RuleFor(x => x.PassengerId)
            .GreaterThan(0).WithMessage("L'identifiant du passager doit être supérieur à zéro.");

        RuleFor(x => x.SeatNumber)
            .NotEmpty().WithMessage("Le numéro du siège est requis.")
            .MaximumLength(10).WithMessage("Le numéro du siège ne peut pas dépasser 10 caractères.");

        RuleFor(x => x.SeatClass)
            .NotEmpty().WithMessage("La classe du siège est requise.")
            .MaximumLength(30).WithMessage("La classe du siège ne peut pas dépasser 30 caractères.");
    }
}