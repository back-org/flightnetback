using Asp.Versioning;
using Flight.Api.Models;
using Flight.Application.CQRS.Commands.Flights;
using Flight.Application.CQRS.Queries.Flights;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des vols.
/// Il permet de consulter, créer, modifier et supprimer des vols.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FlightsController : ParentController
{
    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des vols.
    /// </summary>
    /// <param name="mediator">Médiateur chargé d'exécuter les commandes et requêtes.</param>
    public FlightsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllFlights")]
    [EndpointSummary("Lister tous les vols")]
    [EndpointDescription("Retourne la liste complète des vols enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<FlightDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllFlightsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetFlightById")]
    [EndpointSummary("Obtenir un vol par identifiant")]
    [EndpointDescription("Recherche un vol à partir de son identifiant. Retourne 404 si aucun vol correspondant n'existe.")]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetFlightByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse(
                "Vol introuvable.",
                $"Aucun vol n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreateFlight")]
    [EndpointSummary("Créer un vol")]
    [EndpointDescription("Crée un nouveau vol à partir des données fournies dans le corps de la requête.")]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FlightDto>> Create([FromBody] FlightDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new CreateFlightCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdateFlight")]
    [EndpointSummary("Mettre à jour un vol")]
    [EndpointDescription("Met à jour un vol existant à partir des données fournies.")]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightDto>> Put([FromBody] FlightDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new UpdateFlightCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse(
                "Vol introuvable.",
                $"Aucun vol n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteFlight")]
    [EndpointSummary("Supprimer un vol")]
    [EndpointDescription("Supprime un vol existant. Retourne 404 si l'identifiant n'existe pas.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeleteFlightCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse(
                "Vol introuvable.",
                $"Aucun vol n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}