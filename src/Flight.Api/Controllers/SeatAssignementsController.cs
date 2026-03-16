using Asp.Versioning;
using Flight.Application.CQRS.Commands.SeatAssignments;
using Flight.Application.CQRS.Queries.SeatAssignments;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable des attributions de sièges.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SeatAssignmentsController : ParentController
{
    public SeatAssignmentsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize(Roles = "Admin,BookingAgent,OperationsAgent")]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllSeatAssignmentsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,BookingAgent,OperationsAgent")]
    public async Task<ActionResult<SeatAssignmentDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetSeatAssignmentByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse("Attribution de siège introuvable.", $"Aucune attribution de siège n'a été trouvée avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BookingAgent")]
    public async Task<ActionResult<SeatAssignmentDto>> Create([FromBody] SeatAssignmentDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new CreateSeatAssignmentCommand(dto, User.Identity?.Name ?? "system"));
        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,BookingAgent")]
    public async Task<ActionResult<SeatAssignmentDto>> Put([FromBody] SeatAssignmentDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new UpdateSeatAssignmentCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse("Attribution de siège introuvable.", $"Aucune attribution de siège n'a été trouvée avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,BookingAgent")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(new DeleteSeatAssignmentCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse("Attribution de siège introuvable.", $"Aucune attribution de siège n'a été trouvée avec l'identifiant {id}.");
        }

        return NoContent();
    }
}