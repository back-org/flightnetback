using Asp.Versioning;
using Flight.Application.CQRS.Commands.Notifications;
using Flight.Application.CQRS.Queries.Notifications;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des notifications.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class NotificationsController : ParentController
{
    public NotificationsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllNotificationsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<NotificationDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetNotificationByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse("Notification introuvable.", $"Aucune notification n'a été trouvée avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SupportAgent,BookingAgent")]
    public async Task<ActionResult<NotificationDto>> Create([FromBody] NotificationDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new CreateNotificationCommand(dto, User.Identity?.Name ?? "system"));
        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,SupportAgent,BookingAgent")]
    public async Task<ActionResult<NotificationDto>> Put([FromBody] NotificationDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new UpdateNotificationCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse("Notification introuvable.", $"Aucune notification n'a été trouvée avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(new DeleteNotificationCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse("Notification introuvable.", $"Aucune notification n'a été trouvée avec l'identifiant {id}.");
        }

        return NoContent();
    }
}