/*
 * Rôle métier du fichier: Exposer les endpoints HTTP pour les cas d’usage métier de gestion de vols.
 * Description: Ce fichier participe au sous-domaine 'Flight.Api/Controllers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Asp.Versioning;
using Flight.Application.CQRS.Commands.Users;
using Flight.Application.CQRS.Queries.Users;
using Flight.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des utilisateurs.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ParentController
{
    /// <summary>
    /// Initialise une nouvelle instance du contrôleur utilisateur.
    /// </summary>
    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retourne tous les utilisateurs.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }

    /// <summary>
    /// Retourne un utilisateur à partir de son identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetUserByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse(
                "Utilisateur introuvable.",
                $"Aucun utilisateur n'a été trouvé avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Crée un nouvel utilisateur.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Create([FromBody] UserDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(
            new CreateUserCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    /// <summary>
    /// Met à jour un utilisateur existant.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> Put([FromBody] UserDto dto)
    {
        var invalid = ValidateModel();
        if (invalid is not null) return invalid;

        var result = await Mediator.Send(
            new UpdateUserCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse(
                "Utilisateur introuvable.",
                $"Aucun utilisateur n'a été trouvé avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    /// <summary>
    /// Supprime un utilisateur.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeleteUserCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse(
                "Utilisateur introuvable.",
                $"Aucun utilisateur n'a été trouvé avec l'identifiant {id}.");
        }

        return NoContent();
    }
}