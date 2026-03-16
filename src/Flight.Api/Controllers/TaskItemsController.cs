using Asp.Versioning;
using Flight.Application.CQRS.Commands.TaskItems;
using Flight.Application.CQRS.Queries.TaskItems;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des tâches internes.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TaskItemsController : ParentController
{
    public TaskItemsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllTaskItemsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<TaskItemDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetTaskItemByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse("Tâche introuvable.", $"Aucune tâche n'a été trouvée avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<TaskItemDto>> Create([FromBody] TaskItemDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new CreateTaskItemCommand(dto, User.Identity?.Name ?? "system"));
        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<TaskItemDto>> Put([FromBody] TaskItemDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(new UpdateTaskItemCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse("Tâche introuvable.", $"Aucune tâche n'a été trouvée avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin,SupportAgent,OperationsAgent")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(new DeleteTaskItemCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse("Tâche introuvable.", $"Aucune tâche n'a été trouvée avec l'identifiant {id}.");
        }

        return NoContent();
    }
}