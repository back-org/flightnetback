/*
 * Rôle métier du fichier: Exposer les endpoints HTTP pour les cas d’usage métier de gestion de vols.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Controllers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur de base utilisé par les contrôleurs métier.
/// Il centralise l'accès au médiateur MediatR ainsi que les réponses d'erreur standardisées.
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class ParentController : ControllerBase
{
    /// <summary>
    /// Obtient le médiateur chargé d'exécuter les commandes et requêtes CQRS.
    /// </summary>
    protected IMediator Mediator { get; }

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur parent.
    /// </summary>
    /// <param name="mediator">Médiateur injecté par le conteneur de dépendances.</param>
    protected ParentController(IMediator mediator)
    {
        Mediator = mediator;
    }

    /// <summary>
    /// Construit une réponse HTTP 404 standardisée.
    /// </summary>
    /// <param name="message">Message principal de l'erreur.</param>
    /// <param name="detail">Détail complémentaire de l'erreur.</param>
    /// <returns>Réponse HTTP 404 standardisée.</returns>
    protected NotFoundObjectResult NotFoundResponse(string message, string detail)
    {
        return NotFound(new ErrorResponse
        {
            StatusCode = StatusCodes.Status404NotFound,
            Message = message,
            Detail = detail,
            TraceId = HttpContext.TraceIdentifier
        });
    }

    /// <summary>
    /// Construit une réponse HTTP 400 standardisée.
    /// </summary>
    /// <param name="message">Message principal de l'erreur.</param>
    /// <param name="detail">Détail complémentaire de l'erreur.</param>
    /// <returns>Réponse HTTP 400 standardisée.</returns>
    protected BadRequestObjectResult BadRequestResponse(string message, string detail)
    {
        return BadRequest(new ErrorResponse
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = message,
            Detail = detail,
            TraceId = HttpContext.TraceIdentifier
        });
    }

    /// <summary>
    /// Vérifie si le modèle courant est invalide et retourne une réponse HTTP 400 standardisée.
    /// </summary>
    /// <returns>
    /// Une réponse HTTP 400 si le modèle est invalide ; sinon <c>null</c>.
    /// </returns>
    protected ActionResult? ValidateModel()
    {
        if (ModelState.IsValid)
        {
            return null;
        }

        return BadRequestResponse(
            "Le modèle envoyé est invalide.",
            "Vérifiez les champs obligatoires et les contraintes de validation.");
    }
}