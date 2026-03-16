using Asp.Versioning;
using Flight.Application.CQRS.Commands.Baggages;
using Flight.Application.CQRS.Queries.Baggages;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des bagages.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaggagesController : ParentController
{
    public BaggagesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize(Roles = "Admin,BookingAgent,OperationsAgent,SupportAgent")]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllBaggagesQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,BookingAgent,OperationsAgent,SupportAgent")]
    public async Task<ActionResult<BaggageDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetBaggageByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse("Bagage introuvable.", $"Aucun bagage n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BookingAgent,OperationsAgent")]
    public async Task<ActionResult<BaggageDto>> Create([FromBody] BaggageDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new CreateBaggageCommand(dto, User.Identity?.Name ?? "system"));
        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,BookingAgent,OperationsAgent")]
    public async Task<ActionResult<BaggageDto>> Put([FromBody] BaggageDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new UpdateBaggageCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse("Bagage introuvable.", $"Aucun bagage n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(new DeleteBaggageCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse("Bagage introuvable.", $"Aucun bagage n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}