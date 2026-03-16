/*
 * Rôle métier du fichier: Exposer les endpoints HTTP pour les cas d’usage métier de gestion de vols.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Controllers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Asp.Versioning;
using Flight.Api.Models;
using Flight.Application.CQRS.Commands.Airports;
using Flight.Application.CQRS.Queries.Airports;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des aéroports.
/// Il permet de consulter, créer, modifier et supprimer des aéroports.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AirportsController : ParentController
{
    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des aéroports.
    /// </summary>
    /// <param name="mediator">Médiateur chargé d'exécuter les commandes et requêtes.</param>
    public AirportsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllAirports")]
    [EndpointSummary("Lister tous les aéroports")]
    [EndpointDescription("Retourne la liste complète des aéroports enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<AirportDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllAirportsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetAirportById")]
    [EndpointSummary("Obtenir un aéroport par identifiant")]
    [EndpointDescription("Recherche un aéroport à partir de son identifiant. Retourne 404 si aucun aéroport correspondant n'existe.")]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirportDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetAirportByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse(
                "Aéroport introuvable.",
                $"Aucun aéroport n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointName("CreateAirport")]
    [EndpointSummary("Créer un aéroport")]
    [EndpointDescription("Crée un nouvel aéroport à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AirportDto>> Create([FromBody] AirportDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new CreateAirportCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    [EndpointName("UpdateAirport")]
    [EndpointSummary("Mettre à jour un aéroport")]
    [EndpointDescription("Met à jour un aéroport existant à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirportDto>> Put([FromBody] AirportDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new UpdateAirportCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse(
                "Aéroport introuvable.",
                $"Aucun aéroport n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteAirport")]
    [EndpointSummary("Supprimer un aéroport")]
    [EndpointDescription("Supprime définitivement un aéroport existant. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeleteAirportCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse(
                "Aéroport introuvable.",
                $"Aucun aéroport n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}