/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO refresh token.
/// </summary>
public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
{
    public RefreshTokenDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("L'identifiant utilisateur doit être supérieur à zéro.");

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("La valeur du token est requise.");

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(x => x.CreatedAt)
            .WithMessage("La date d'expiration doit être postérieure à la date de création.");
    }
}