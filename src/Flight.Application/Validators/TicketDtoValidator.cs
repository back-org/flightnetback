/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO billet.
/// Vérifie que les informations essentielles d'un billet sont valides.
/// </summary>
public class TicketDtoValidator : AbstractValidator<TicketDto>
{
    public TicketDtoValidator()
    {
        RuleFor(x => x.TicketNumber)
            .NotEmpty().WithMessage("Le numéro du billet est requis.")
            .MaximumLength(50).WithMessage("Le numéro du billet ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.BookingId)
            .GreaterThan(0).WithMessage("L'identifiant de la réservation doit être supérieur à zéro.");

        RuleFor(x => x.PassengerId)
            .GreaterThan(0).WithMessage("L'identifiant du passager doit être supérieur à zéro.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Le statut du billet est requis.")
            .MaximumLength(30).WithMessage("Le statut du billet ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.IssuedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("La date d'émission ne peut pas être dans le futur.");
    }
}