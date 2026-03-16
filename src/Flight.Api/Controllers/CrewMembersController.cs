/*
 * Rôle métier du fichier: Exposer les endpoints HTTP pour les cas d’usage métier de gestion de vols.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Controllers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Asp.Versioning;
using Flight.Application.CQRS.Commands.CrewMembers;
using Flight.Application.CQRS.Queries.CrewMembers;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des membres d'équipe et d'équipage.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CrewMembersController : ParentController
{
    public CrewMembersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize(Roles = "Admin,OperationsAgent")]
    [ProducesResponseType(typeof(IEnumerable<CrewMemberDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllCrewMembersQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,OperationsAgent")]
    [ProducesResponseType(typeof(CrewMemberDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CrewMemberDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetCrewMemberByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse("Membre d'équipe introuvable.", $"Aucun membre d'équipe n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,OperationsAgent")]
    [ProducesResponseType(typeof(CrewMemberDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CrewMemberDto>> Create([FromBody] CrewMemberDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(
            new CreateCrewMemberCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,OperationsAgent")]
    [ProducesResponseType(typeof(CrewMemberDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CrewMemberDto>> Put([FromBody] CrewMemberDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(
            new UpdateCrewMemberCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse("Membre d'équipe introuvable.", $"Aucun membre d'équipe n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeleteCrewMemberCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse("Membre d'équipe introuvable.", $"Aucun membre d'équipe n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}