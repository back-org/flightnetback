using Asp.Versioning;
using Flight.Application.CQRS.Commands.Aircrafts;
using Flight.Application.CQRS.Queries.Aircrafts;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des avions de la flotte.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AircraftsController : ParentController
{
    public AircraftsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize(Roles = "Admin,OperationsAgent")]
    [ProducesResponseType(typeof(IEnumerable<AircraftDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllAircraftsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,OperationsAgent")]
    [ProducesResponseType(typeof(AircraftDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AircraftDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetAircraftByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse("Avion introuvable.", $"Aucun avion n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,OperationsAgent")]
    [ProducesResponseType(typeof(AircraftDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<AircraftDto>> Create([FromBody] AircraftDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(
            new CreateAircraftCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,OperationsAgent")]
    [ProducesResponseType(typeof(AircraftDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<AircraftDto>> Put([FromBody] AircraftDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(
            new UpdateAircraftCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse("Avion introuvable.", $"Aucun avion n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeleteAircraftCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse("Avion introuvable.", $"Aucun avion n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}