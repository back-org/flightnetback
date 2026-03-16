/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour l'objet <see cref="AirportDto"/>.
/// </summary>
public class AirportDtoValidator : AbstractValidator<AirportDto>
{
    /// <summary>
    /// Initialise les règles de validation pour <see cref="AirportDto"/>.
    /// </summary>
    public AirportDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Le nom de l'aéroport est requis.")
            .MaximumLength(30)
            .WithMessage("Le nom de l'aéroport ne peut pas dépasser 30 caractères.");

        RuleFor(x => x.State)
            .IsInEnum()
            .WithMessage("L'état de l'aéroport est invalide.");

        RuleFor(x => x.DeletedFlag)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Le drapeau de suppression logique doit être supérieur ou égal à 0.");
    }
}