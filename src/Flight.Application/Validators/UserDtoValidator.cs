/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO utilisateur.
/// Vérifie que les informations minimales d'un utilisateur sont correctes.
/// </summary>
public class UserDtoValidator : AbstractValidator<UserDto>
{
    /// <summary>
    /// Initialise les règles de validation du DTO utilisateur.
    /// </summary>
    public UserDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Le nom d'utilisateur est requis.")
            .MaximumLength(50).WithMessage("Le nom d'utilisateur ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'adresse e-mail est requise.")
            .EmailAddress().WithMessage("L'adresse e-mail n'est pas valide.")
            .MaximumLength(100).WithMessage("L'adresse e-mail ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.FirstName)
            .MaximumLength(50).WithMessage("Le prénom ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.LastName)
            .MaximumLength(50).WithMessage("Le nom ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(30).WithMessage("Le numéro de téléphone ne peut pas dépasser 30 caractères.");
    }
}