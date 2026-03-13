using Asp.Versioning;
using Flight.Api.Models;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des vols.
/// Il permet de consulter, créer, modifier et supprimer des vols.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class FlightsController : ParentController
{
    private readonly Flight.Domain.Interfaces.IGenericRepository<Flight.Domain.Entities.Flight> _repository;

    public FlightsController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.Flight;
    }

    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllFlights")]
    [EndpointSummary("Lister tous les vols")]
    [EndpointDescription("Retourne la liste complète des vols enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<FlightDto>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items.Select(x => x.ToDto()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetFlightById")]
    [EndpointSummary("Obtenir un vol par identifiant")]
    [EndpointDescription("Recherche un vol à partir de son identifiant. Retourne 404 si aucun vol correspondant n'existe.")]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightDto>> Get([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Vol introuvable.",
                Detail = $"Aucun vol n'a été trouvé avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreateFlight")]
    [EndpointSummary("Créer un vol")]
    [EndpointDescription("Crée un nouveau vol à partir des données fournies dans le corps de la requête.")]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FlightDto>> Create([FromBody] FlightDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Le modèle envoyé est invalide.",
                Detail = "Vérifiez les champs obligatoires et les règles de validation.",
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
                Message = "La création du vol a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdateFlight")]
    [EndpointSummary("Mettre à jour un vol")]
    [EndpointDescription("Met à jour un vol existant à partir des données fournies.")]
    [ProducesResponseType(typeof(FlightDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FlightDto>> Put([FromBody] FlightDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Le modèle envoyé est invalide.",
                Detail = "Vérifiez les champs obligatoires et les règles de validation.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        var item = await _repository.GetByIdAsync(dto.Id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Vol introuvable.",
                Detail = $"Aucun vol n'a été trouvé avec l'identifiant {dto.Id}.",
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
                Message = "La mise à jour du vol a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteFlight")]
    [EndpointSummary("Supprimer un vol")]
    [EndpointDescription("Supprime un vol existant. Retourne 404 si l'identifiant n'existe pas.")]
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
                Message = "Vol introuvable.",
                Detail = $"Aucun vol n'a été trouvé avec l'identifiant {id}.",
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
                Message = "La suppression du vol a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }
}