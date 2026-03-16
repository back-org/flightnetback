/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour l'objet <see cref="BookingDto"/>.
/// </summary>
public class BookingDtoValidator : AbstractValidator<BookingDto>
{
    /// <summary>
    /// Initialise les règles de validation pour <see cref="BookingDto"/>.
    /// </summary>
    public BookingDtoValidator()
    {
        RuleFor(x => x.FlightId)
            .GreaterThan(0)
            .WithMessage("L'identifiant du vol doit être supérieur à 0.");

        RuleFor(x => x.PassengerId)
            .GreaterThan(0)
            .WithMessage("L'identifiant du passager doit être supérieur à 0.");

        RuleFor(x => x.FlightType)
            .IsInEnum()
            .WithMessage("La classe de confort spécifiée est invalide.");

        RuleFor(x => x.Statut)
            .IsInEnum()
            .WithMessage("Le statut de réservation spécifié est invalide.");
    }
}