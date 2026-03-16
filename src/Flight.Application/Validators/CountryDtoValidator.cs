/*
 * Rôle métier du fichier: Appliquer les règles de validation métier et fonctionnelle.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Validators' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

﻿using Flight.Application.DTOs;
using FluentValidation;

namespace Flight.Application.Validators;

/// <summary>
/// Validateur FluentValidation pour l'objet <see cref="CountryDto"/>.
/// </summary>
public class CountryDtoValidator : AbstractValidator<CountryDto>
{
    /// <summary>
    /// Initialise les règles de validation pour <see cref="CountryDto"/>.
    /// </summary>
    public CountryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Le nom du pays est requis.");

        RuleFor(x => x.Iso2)
            .NotEmpty()
            .WithMessage("Le code ISO2 est requis.")
            .Length(2)
            .WithMessage("Le code ISO2 doit contenir exactement 2 caractères.");

        RuleFor(x => x.Iso3)
            .NotEmpty()
            .WithMessage("Le code ISO3 est requis.")
            .Length(3)
            .WithMessage("Le code ISO3 doit contenir exactement 3 caractères.");
    }
}