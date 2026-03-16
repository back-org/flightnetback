/*
 * Rôle métier du fichier: Exposer les endpoints HTTP pour les cas d’usage métier de gestion de vols.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Controllers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Asp.Versioning;
using Flight.Api.Models;
using Flight.Application.CQRS.Commands.Airlines;
using Flight.Application.CQRS.Queries.Airlines;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des compagnies aériennes.
/// Il permet de consulter, créer, modifier et supprimer des compagnies aériennes.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AirlinesController : ParentController
{
    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des compagnies aériennes.
    /// </summary>
    /// <param name="mediator">Médiateur chargé d'exécuter les commandes et requêtes.</param>
    public AirlinesController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retourne la liste complète des compagnies aériennes enregistrées.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllAirlines")]
    [EndpointSummary("Lister toutes les compagnies aériennes")]
    [EndpointDescription("Retourne la liste complète des compagnies aériennes enregistrées dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<AirlineDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllAirlinesQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retourne le détail d'une compagnie aérienne à partir de son identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetAirlineById")]
    [EndpointSummary("Obtenir une compagnie aérienne par identifiant")]
    [EndpointDescription("Recherche une compagnie aérienne à partir de son identifiant. Retourne 404 si aucune compagnie correspondante n'existe.")]
    [ProducesResponseType(typeof(AirlineDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirlineDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetAirlineByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse(
                "Compagnie aérienne introuvable.",
                $"Aucune compagnie aérienne n'a été trouvée avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Crée une nouvelle compagnie aérienne.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointName("CreateAirline")]
    [EndpointSummary("Créer une compagnie aérienne")]
    [EndpointDescription("Crée une nouvelle compagnie aérienne à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(AirlineDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AirlineDto>> Create([FromBody] AirlineDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new CreateAirlineCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    /// <summary>
    /// Met à jour une compagnie aérienne existante.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [EndpointName("UpdateAirline")]
    [EndpointSummary("Mettre à jour une compagnie aérienne")]
    [EndpointDescription("Met à jour une compagnie aérienne existante à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(AirlineDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirlineDto>> Put([FromBody] AirlineDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new UpdateAirlineCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse(
                "Compagnie aérienne introuvable.",
                $"Aucune compagnie aérienne n'a été trouvée avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Supprime définitivement une compagnie aérienne.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteAirline")]
    [EndpointSummary("Supprimer une compagnie aérienne")]
    [EndpointDescription("Supprime définitivement une compagnie aérienne existante. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeleteAirlineCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse(
                "Compagnie aérienne introuvable.",
                $"Aucune compagnie aérienne n'a été trouvée avec l'identifiant {id}.");
        }

        return NoContent();
    }
}