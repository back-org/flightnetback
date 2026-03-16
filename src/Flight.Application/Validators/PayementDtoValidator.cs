/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO paiement.
/// Vérifie qu'un paiement possède des données de base cohérentes.
/// </summary>
public class PaymentDtoValidator : AbstractValidator<PaymentDto>
{
    /// <summary>
    /// Initialise les règles de validation du DTO paiement.
    /// </summary>
    public PaymentDtoValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0).WithMessage("L'identifiant de la réservation doit être supérieur à zéro.");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Le montant du paiement doit être positif ou nul.");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("La devise est requise.")
            .MaximumLength(10).WithMessage("La devise ne peut pas dépasser 10 caractères.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("La méthode de paiement est requise.")
            .MaximumLength(30).WithMessage("La méthode de paiement ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Le statut du paiement est requis.")
            .MaximumLength(30).WithMessage("Le statut du paiement ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.TransactionReference)
            .MaximumLength(100).WithMessage("La référence de transaction ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.PaidAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.PaidAt.HasValue)
            .WithMessage("La date de paiement ne peut pas être dans le futur.");
    }
}