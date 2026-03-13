using Asp.Versioning;
using Flight.Api.Models;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des passagers.
/// Il permet de consulter, créer, modifier et supprimer des passagers.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class PassengersController : ParentController
{
    private readonly IGenericRepository<Passenger> _repository;

    public PassengersController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.Passenger;
    }

    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllPassengers")]
    [EndpointSummary("Lister tous les passagers")]
    [EndpointDescription("Retourne la liste complète des passagers enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<PassengerDto>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items.Select(x => x.ToDto()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetPassengerById")]
    [EndpointSummary("Obtenir un passager par identifiant")]
    [EndpointDescription("Recherche un passager à partir de son identifiant. Retourne 404 si aucun passager correspondant n'existe.")]
    [ProducesResponseType(typeof(PassengerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PassengerDto>> Get([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Passager introuvable.",
                Detail = $"Aucun passager n'a été trouvé avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreatePassenger")]
    [EndpointSummary("Créer un passager")]
    [EndpointDescription("Crée un nouveau passager à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(PassengerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PassengerDto>> Create([FromBody] PassengerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Le modèle envoyé est invalide.",
                Detail = "Vérifiez les champs obligatoires et les contraintes de validation.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            var entity = dto.ToEntity();
            await _repository.AddAsync(entity);

            return CreatedAtAction(nameof(Get), new { version = "1.0", id = entity.Id }, entity.ToDto());
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La création du passager a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdatePassenger")]
    [EndpointSummary("Mettre à jour un passager")]
    [EndpointDescription("Met à jour un passager existant à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(PassengerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PassengerDto>> Put([FromBody] PassengerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Le modèle envoyé est invalide.",
                Detail = "Vérifiez les champs obligatoires et les contraintes de validation.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        var item = await _repository.GetByIdAsync(dto.Id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Passager introuvable.",
                Detail = $"Aucun passager n'a été trouvé avec l'identifiant {dto.Id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            item.UpdateEntity(dto);
            await _repository.Update(item);

            return Ok(item.ToDto());
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La mise à jour du passager a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeletePassenger")]
    [EndpointSummary("Supprimer un passager")]
    [EndpointDescription("Supprime définitivement un passager existant. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Passager introuvable.",
                Detail = $"Aucun passager n'a été trouvé avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La suppression du passager a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }
}