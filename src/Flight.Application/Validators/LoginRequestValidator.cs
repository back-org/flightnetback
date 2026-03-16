/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Modèle de requête de connexion (dupliqué pour validation isolée).
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequestModel>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Le nom d'utilisateur est requis.")
            .MinimumLength(3).WithMessage("Le nom d'utilisateur doit contenir au moins 3 caractères.")
            .MaximumLength(50).WithMessage("Le nom d'utilisateur ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Le mot de passe est requis.")
            .MinimumLength(6).WithMessage("Le mot de passe doit contenir au moins 6 caractères.");
    }
}

/// <summary>
/// DTO de requête de connexion utilisé pour la validation.
/// </summary>
public record LoginRequestModel(string UserName, string Password);
