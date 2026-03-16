/*
 * Rôle métier du fichier: Composant applicatif.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/Behaviors' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Flight.Application.Behaviors;

/// <summary>
/// Behaviour MediatR qui exécute automatiquement les validateurs FluentValidation
/// avant chaque commande ou requête.
/// </summary>
/// <typeparam name="TRequest">Type de la requête MediatR.</typeparam>
/// <typeparam name="TResponse">Type de la réponse MediatR.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            _logger.LogWarning(
                "Validation échouée pour {RequestType}: {@Failures}",
                typeof(TRequest).Name,
                failures.Select(f => new { f.PropertyName, f.ErrorMessage }));

            throw new ValidationException(failures);
        }

        return await next();
    }
}
