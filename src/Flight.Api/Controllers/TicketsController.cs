/*
 * Rôle métier du fichier: Exposer les endpoints HTTP pour les cas d’usage métier de gestion de vols.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Controllers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Asp.Versioning;
using Flight.Application.CQRS.Commands.Tickets;
using Flight.Application.CQRS.Queries.Tickets;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des billets.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TicketsController : ParentController
{
    public TicketsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize(Roles = "Admin,BookingAgent,SupportAgent")]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllTicketsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,BookingAgent,SupportAgent")]
    public async Task<ActionResult<TicketDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetTicketByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse("Billet introuvable.", $"Aucun billet n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BookingAgent")]
    public async Task<ActionResult<TicketDto>> Create([FromBody] TicketDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new CreateTicketCommand(dto, User.Identity?.Name ?? "system"));
        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,BookingAgent")]
    public async Task<ActionResult<TicketDto>> Put([FromBody] TicketDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new UpdateTicketCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse("Billet introuvable.", $"Aucun billet n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(new DeleteTicketCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse("Billet introuvable.", $"Aucun billet n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}