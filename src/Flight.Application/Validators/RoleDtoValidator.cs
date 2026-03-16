/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur du DTO rôle.
/// Vérifie que le rôle possède au minimum un nom valide.
/// </summary>
public class RoleDtoValidator : AbstractValidator<RoleDto>
{
    /// <summary>
    /// Initialise les règles de validation du DTO rôle.
    /// </summary>
    public RoleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom du rôle est requis.")
            .MaximumLength(50).WithMessage("Le nom du rôle ne peut pas dépasser 50 caractères.");

        RuleFor(x => x.Description)
            .MaximumLength(200).WithMessage("La description ne peut pas dépasser 200 caractères.");
    }
}