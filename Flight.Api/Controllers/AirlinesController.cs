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
/// Contrôleur responsable de la gestion des compagnies aériennes.
/// Il permet de consulter, créer, modifier et supprimer des compagnies aériennes.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class AirlinesController : ParentController
{
    private readonly IGenericRepository<Airline> _repository;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des compagnies aériennes.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories injecté par l'application.</param>
    public AirlinesController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.Airline;
    }

    /// <summary>
    /// Retourne la liste complète des compagnies aériennes enregistrées.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllAirlines")]
    [EndpointSummary("Lister toutes les compagnies aériennes")]
    [EndpointDescription("Retourne la liste complète des compagnies aériennes enregistrées dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<AirlineDto>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items.Select(x => x.ToDto()));
    }

    /// <summary>
    /// Retourne le détail d'une compagnie aérienne à partir de son identifiant.
    /// </summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetAirlineById")]
    [EndpointSummary("Obtenir une compagnie aérienne par identifiant")]
    [EndpointDescription("Recherche une compagnie aérienne à partir de son identifiant. Retourne 404 si elle n'existe pas.")]
    [ProducesResponseType(typeof(AirlineDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirlineDto>> Get([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Compagnie aérienne introuvable.",
                Detail = $"Aucune compagnie aérienne n'a été trouvée avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item.ToDto());
    }

    /// <summary>
    /// Crée une nouvelle compagnie aérienne.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointName("CreateAirline")]
    [EndpointSummary("Créer une compagnie aérienne")]
    [EndpointDescription("Crée une nouvelle compagnie aérienne à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(AirlineDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AirlineDto>> Create([FromBody] AirlineDto dto)
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
                Message = "La création de la compagnie aérienne a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Met à jour une compagnie aérienne existante.
    /// </summary>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [EndpointName("UpdateAirline")]
    [EndpointSummary("Mettre à jour une compagnie aérienne")]
    [EndpointDescription("Met à jour une compagnie aérienne existante à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(AirlineDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AirlineDto>> Put([FromBody] AirlineDto dto)
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
                Message = "Compagnie aérienne introuvable.",
                Detail = $"Aucune compagnie aérienne n'a été trouvée avec l'identifiant {dto.Id}.",
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
                Message = "La mise à jour de la compagnie aérienne a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Supprime définitivement une compagnie aérienne.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteAirline")]
    [EndpointSummary("Supprimer une compagnie aérienne")]
    [EndpointDescription("Supprime définitivement une compagnie aérienne existante. Endpoint réservé aux administrateurs.")]
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
                Message = "Compagnie aérienne introuvable.",
                Detail = $"Aucune compagnie aérienne n'a été trouvée avec l'identifiant {id}.",
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
                Message = "La suppression de la compagnie aérienne a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }
}