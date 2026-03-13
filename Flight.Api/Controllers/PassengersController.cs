using Asp.Versioning;
using Flight.Api.Models;
using Flight.Application.CQRS.Commands.Passengers;
using Flight.Application.CQRS.Queries.Passengers;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des passagers.
/// Il permet de consulter, créer, modifier et supprimer des passagers.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PassengersController : ParentController
{
    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des passagers.
    /// </summary>
    /// <param name="mediator">Médiateur chargé d'exécuter les commandes et requêtes.</param>
    public PassengersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllPassengers")]
    [EndpointSummary("Lister tous les passagers")]
    [EndpointDescription("Retourne la liste complète des passagers enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<PassengerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllPassengersQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetPassengerById")]
    [EndpointSummary("Obtenir un passager par identifiant")]
    [EndpointDescription("Recherche un passager à partir de son identifiant. Retourne 404 si aucun passager correspondant n'existe.")]
    [ProducesResponseType(typeof(PassengerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PassengerDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetPassengerByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse(
                "Passager introuvable.",
                $"Aucun passager n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreatePassenger")]
    [EndpointSummary("Créer un passager")]
    [EndpointDescription("Crée un nouveau passager à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(PassengerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PassengerDto>> Create([FromBody] PassengerDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new CreatePassengerCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdatePassenger")]
    [EndpointSummary("Mettre à jour un passager")]
    [EndpointDescription("Met à jour un passager existant à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(PassengerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PassengerDto>> Put([FromBody] PassengerDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new UpdatePassengerCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse(
                "Passager introuvable.",
                $"Aucun passager n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeletePassenger")]
    [EndpointSummary("Supprimer un passager")]
    [EndpointDescription("Supprime définitivement un passager existant. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeletePassengerCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse(
                "Passager introuvable.",
                $"Aucun passager n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}