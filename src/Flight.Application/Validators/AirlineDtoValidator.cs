/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour l'objet <see cref="AirlineDto"/>.
/// </summary>
public class AirlineDtoValidator : AbstractValidator<AirlineDto>
{
    /// <summary>
    /// Initialise les règles de validation pour <see cref="AirlineDto"/>.
    /// </summary>
    public AirlineDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Le nom de la compagnie aérienne est requis.")
            .MaximumLength(30)
            .WithMessage("Le nom de la compagnie aérienne ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.State)
            .IsInEnum()
            .WithMessage("L'état de la compagnie aérienne est invalide.");

        RuleFor(x => x.DeletedFlag)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Le drapeau de suppression logique doit être supérieur ou égal à 0.");
    }
}