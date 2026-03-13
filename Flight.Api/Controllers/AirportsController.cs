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
/// Contrôleur responsable de la gestion des aéroports.
/// Il permet de consulter, créer, modifier et supprimer des aéroports.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class AirportsController : ParentController
{
    private readonly IGenericRepository<Airport> _repository;

    public AirportsController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.Airport;
    }

    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllAirports")]
    [EndpointSummary("Lister tous les aéroports")]
    [EndpointDescription("Retourne la liste complète des aéroports enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<AirportDto>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items.Select(x => x.ToDto()));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetAirportById")]
    [EndpointSummary("Obtenir un aéroport par identifiant")]
    [EndpointDescription("Recherche un aéroport à partir de son identifiant. Retourne 404 si aucun aéroport correspondant n'existe.")]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirportDto>> Get([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Aéroport introuvable.",
                Detail = $"Aucun aéroport n'a été trouvé avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item.ToDto());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointName("CreateAirport")]
    [EndpointSummary("Créer un aéroport")]
    [EndpointDescription("Crée un nouvel aéroport à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AirportDto>> Create([FromBody] AirportDto dto)
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
                Message = "La création de l'aéroport a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    [EndpointName("UpdateAirport")]
    [EndpointSummary("Mettre à jour un aéroport")]
    [EndpointDescription("Met à jour un aéroport existant à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(AirportDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirportDto>> Put([FromBody] AirportDto dto)
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
                Message = "Aéroport introuvable.",
                Detail = $"Aucun aéroport n'a été trouvé avec l'identifiant {dto.Id}.",
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
                Message = "La mise à jour de l'aéroport a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteAirport")]
    [EndpointSummary("Supprimer un aéroport")]
    [EndpointDescription("Supprime définitivement un aéroport existant. Endpoint réservé aux administrateurs.")]
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
                Message = "Aéroport introuvable.",
                Detail = $"Aucun aéroport n'a été trouvé avec l'identifiant {id}.",
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
                Message = "La suppression de l'aéroport a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }
}