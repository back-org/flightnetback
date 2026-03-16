/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO notification.
/// </summary>
public class NotificationDtoValidator : AbstractValidator<NotificationDto>
{
    public NotificationDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("L'identifiant utilisateur doit être supérieur à zéro.");

        RuleFor(x => x.Subject)
            .MaximumLength(100).WithMessage("Le sujet ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Le message de notification est requis.");

        RuleFor(x => x.Channel)
            .NotEmpty().WithMessage("Le canal de notification est requis.")
            .MaximumLength(30).WithMessage("Le canal ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Le statut de la notification est requis.")
            .MaximumLength(30).WithMessage("Le statut de la notification ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("La date de création ne peut pas être dans le futur.");

        RuleFor(x => x.SentAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.SentAt.HasValue)
            .WithMessage("La date d'envoi ne peut pas être dans le futur.");
    }
}