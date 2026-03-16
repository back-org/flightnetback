using Asp.Versioning;
using Flight.Application.CQRS.Commands.Roles;
using Flight.Application.CQRS.Queries.Roles;
using Flight.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des rôles.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RolesController : ParentController
{
    /// <summary>
    /// Initialise une nouvelle instance du contrôleur rôle.
    /// </summary>
    public RolesController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retourne tous les rôles.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<RoleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllRolesQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retourne un rôle à partir de son identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetRoleByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse(
                "Rôle introuvable.",
                $"Aucun rôle n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Crée un nouveau rôle.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleDto>> Create([FromBody] RoleDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(
            new CreateRoleCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    /// <summary>
    /// Met à jour un rôle existant.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(RoleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> Put([FromBody] RoleDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(
            new UpdateRoleCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse(
                "Rôle introuvable.",
                $"Aucun rôle n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Supprime un rôle.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeleteRoleCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse(
                "Rôle introuvable.",
                $"Aucun rôle n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}