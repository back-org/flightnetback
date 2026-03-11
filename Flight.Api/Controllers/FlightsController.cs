using Flight.Api.Models;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur gérant les opérations CRUD et la consultation paginée des vols.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FlightsController : ParentController
{
    private readonly IGenericRepository<Domain.Entities.Flight> _flightRepository;
    private readonly ILogger<FlightsController> _logger;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des vols.
    /// </summary>
    /// <param name="manager">Gestionnaire de repositories.</param>
    /// <param name="logger">Service de journalisation.</param>
    public FlightsController(
        IRepositoryManager manager,
        ILogger<FlightsController> logger) : base(manager)
    {
        _flightRepository = Manager.Flight;
        _logger = logger;
    }

    /// <summary>
    /// Retourne tous les vols.
    /// </summary>
    /// <returns>Une réponse HTTP 200 contenant la liste complète des vols.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FlightDto>>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var flights = await _flightRepository.AllAsync();
        var response = ApiResponse<IEnumerable<FlightDto>>.Ok(
            flights.Select(x => x.ToDto()).ToList(),
            "Liste des vols récupérée avec succès.",
            HttpContext.TraceIdentifier);

        return Ok(response);
    }

    /// <summary>
    /// Retourne une liste paginée de vols.
    /// </summary>
    /// <param name="query">Les paramètres de pagination.</param>
    /// <returns>Une réponse paginée contenant les vols.</returns>
    [HttpGet("paged")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FlightDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] PagedQuery query)
    {
        var items = await _flightRepository.SelectAllByPageAsync(query.PageNumber, query.PageSize);
        var totalCount = await _flightRepository.CountAsync();

        var result = new PagedResult<FlightDto>
        {
            Items = items.Select(x => x.ToDto()).ToList(),
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };

        return Ok(ApiResponse<PagedResult<FlightDto>>.Ok(
            result,
            "Vols paginés récupérés avec succès.",
            HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Retourne un vol par son identifiant.
    /// </summary>
    /// <param name="id">Identifiant du vol.</param>
    /// <returns>Le vol demandé si trouvé, sinon 404.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<FlightDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var flight = await _flightRepository.GetByIdAsync(id);

        if (flight is null)
        {
            return NotFound(ApiResponse<string>.Fail(
                $"Aucun vol trouvé pour l'identifiant {id}.",
                HttpContext.TraceIdentifier));
        }

        return Ok(ApiResponse<FlightDto>.Ok(
            flight.ToDto(),
            "Vol récupéré avec succès.",
            HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Crée un nouveau vol.
    /// </summary>
    /// <param name="dto">Données du vol à créer.</param>
    /// <returns>Le vol créé avec code HTTP 201.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<FlightDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] FlightDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponse<string>.Fail(
                "Le corps de la requête est invalide.",
                HttpContext.TraceIdentifier));
        }

        var entity = new Domain.Entities.Flight(dto);

        await _flightRepository.AddAsync(entity);

        _logger.LogInformation("Vol créé avec succès. Id={FlightId}, Code={Code}", entity.Id, entity.Code);

        return CreatedAtAction(
            nameof(Get),
            new { id = entity.Id },
            ApiResponse<FlightDto>.Ok(
                entity.ToDto(),
                "Vol créé avec succès.",
                HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Met à jour un vol existant.
    /// </summary>
    /// <param name="id">Identifiant du vol à mettre à jour.</param>
    /// <param name="dto">Données de mise à jour.</param>
    /// <returns>Le vol mis à jour ou une erreur métier.</returns>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<FlightDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] FlightDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(ApiResponse<string>.Fail(
                "L'identifiant de l'URL ne correspond pas à celui du payload.",
                HttpContext.TraceIdentifier));
        }

        var existing = await _flightRepository.GetByIdAsync(id);

        if (existing is null)
        {
            return NotFound(ApiResponse<string>.Fail(
                $"Aucun vol trouvé pour l'identifiant {id}.",
                HttpContext.TraceIdentifier));
        }

        existing.Copy(dto);
        await _flightRepository.Update(existing);

        _logger.LogInformation("Vol mis à jour avec succès. Id={FlightId}", existing.Id);

        return Ok(ApiResponse<FlightDto>.Ok(
            existing.ToDto(),
            "Vol mis à jour avec succès.",
            HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Supprime un vol.
    /// </summary>
    /// <param name="id">Identifiant du vol à supprimer.</param>
    /// <returns>Une réponse 204 si la suppression a réussi.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _flightRepository.GetByIdAsync(id);

        if (existing is null)
        {
            return NotFound(ApiResponse<string>.Fail(
                $"Aucun vol trouvé pour l'identifiant {id}.",
                HttpContext.TraceIdentifier));
        }

        await _flightRepository.Delete(existing);

        _logger.LogInformation("Vol supprimé avec succès. Id={FlightId}", id);

        return NoContent();
    }
}