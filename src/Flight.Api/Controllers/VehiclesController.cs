/*
 * Rôle métier du fichier: Exposer les endpoints HTTP pour les cas d’usage métier de gestion de vols.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Controllers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Asp.Versioning;
using Flight.Api.Models;
using Flight.Application.CQRS.Commands.Vehicles;
using Flight.Application.CQRS.Queries.Vehicles;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des véhicules.
/// Il permet de consulter, créer, modifier et supprimer des véhicules.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class VehiclesController : ParentController
{
    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des véhicules.
    /// </summary>
    /// <param name="mediator">Médiateur chargé d'exécuter les commandes et requêtes.</param>
    public VehiclesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllVehicles")]
    [EndpointSummary("Lister tous les véhicules")]
    [EndpointDescription("Retourne la liste complète des véhicules enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<VehicleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllVehiclesQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetVehicleById")]
    [EndpointSummary("Obtenir un véhicule par identifiant")]
    [EndpointDescription("Recherche un véhicule à partir de son identifiant. Retourne 404 si aucun véhicule correspondant n'existe.")]
    [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetVehicleByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse(
                "Véhicule introuvable.",
                $"Aucun véhicule n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreateVehicle")]
    [EndpointSummary("Créer un véhicule")]
    [EndpointDescription("Crée un nouveau véhicule à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VehicleDto>> Create([FromBody] VehicleDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new CreateVehicleCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdateVehicle")]
    [EndpointSummary("Mettre à jour un véhicule")]
    [EndpointDescription("Met à jour un véhicule existant à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<VehicleDto>> Put([FromBody] VehicleDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new UpdateVehicleCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse(
                "Véhicule introuvable.",
                $"Aucun véhicule n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteVehicle")]
    [EndpointSummary("Supprimer un véhicule")]
    [EndpointDescription("Supprime définitivement un véhicule existant. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeleteVehicleCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse(
                "Véhicule introuvable.",
                $"Aucun véhicule n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}