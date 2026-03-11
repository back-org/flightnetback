using Flight.Api.Models;
using Flight.Application.CQRS.Commands.Flights;
using Flight.Application.CQRS.Queries.Flights;
using Flight.Domain.Entities;
using Flight.Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur gérant les opérations CRUD et la consultation paginée des vols.
/// </summary>
[Asp.Versioning.ApiVersion("1.0")]
[Asp.Versioning.ApiVersion("2.0")]
public class FlightsController : ParentController
{
    private readonly IMediator _mediator;
    private readonly ILogger<FlightsController> _logger;
    private readonly IAuditTrailService _audit;

    public FlightsController(
        IRepositoryManager manager,
        IMediator mediator,
        ILogger<FlightsController> logger,
        IAuditTrailService audit) : base(manager)
    {
        _mediator = mediator;
        _logger = logger;
        _audit = audit;
    }

    /// <summary>Retourne tous les vols.</summary>
    [HttpGet]
    [AllowAnonymous]
    [Asp.Versioning.ApiVersion("1.0")]
    [Asp.Versioning.ApiVersion("2.0")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FlightDto>>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var flights = await _mediator.Send(new GetAllFlightsQuery());
        return Ok(ApiResponse<IEnumerable<FlightDto>>.Ok(
            flights,
            "Liste des vols récupérée avec succès.",
            HttpContext.TraceIdentifier));
    }

    /// <summary>Retourne une liste paginée de vols.</summary>
    [HttpGet("paged")]
    [AllowAnonymous]
    [Asp.Versioning.ApiVersion("1.0")]
    [Asp.Versioning.ApiVersion("2.0")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<FlightDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] PagedQuery query)
    {
        var items = await Manager.Flight.SelectAllByPageAsync(query.PageNumber, query.PageSize);
        var totalCount = await Manager.Flight.CountAsync();

        var result = new PagedResult<FlightDto>
        {
            Items = items.Select(x => x.ToDto()).ToList(),
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };

        return Ok(ApiResponse<PagedResult<FlightDto>>.Ok(
            result, "Vols paginés récupérés avec succès.", HttpContext.TraceIdentifier));
    }

    /// <summary>Retourne un vol par son identifiant.</summary>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<FlightDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var flight = await _mediator.Send(new GetFlightByIdQuery(id));
        if (flight is null)
            return NotFound(ApiResponse<string>.Fail(
                $"Aucun vol trouvé pour l'identifiant {id}.", HttpContext.TraceIdentifier));

        return Ok(ApiResponse<FlightDto>.Ok(flight, "Vol récupéré avec succès.", HttpContext.TraceIdentifier));
    }

    /// <summary>Crée un nouveau vol. (Admin uniquement)</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("write")]
    [ProducesResponseType(typeof(ApiResponse<FlightDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] FlightDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<string>.Fail(
                "Le corps de la requête est invalide.", HttpContext.TraceIdentifier));

        var created = await _mediator.Send(new CreateFlightCommand(dto));

        return CreatedAtAction(nameof(Get), new { id = created.Id },
            ApiResponse<FlightDto>.Ok(created, "Vol créé avec succès.", HttpContext.TraceIdentifier));
    }

    /// <summary>Met à jour un vol existant. (Admin uniquement)</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("write")]
    [ProducesResponseType(typeof(ApiResponse<FlightDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] FlightDto dto)
    {
        if (id != dto.Id)
            return BadRequest(ApiResponse<string>.Fail(
                "L'identifiant de l'URL ne correspond pas à celui du payload.",
                HttpContext.TraceIdentifier));

        var updated = await _mediator.Send(new UpdateFlightCommand(id, dto));
        if (updated is null)
            return NotFound(ApiResponse<string>.Fail(
                $"Aucun vol trouvé pour l'identifiant {id}.", HttpContext.TraceIdentifier));

        return Ok(ApiResponse<FlightDto>.Ok(updated, "Vol mis à jour avec succès.", HttpContext.TraceIdentifier));
    }

    /// <summary>Supprime un vol. (Admin uniquement)</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EnableRateLimiting("write")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteFlightCommand(id));
        if (!deleted)
            return NotFound(ApiResponse<string>.Fail(
                $"Aucun vol trouvé pour l'identifiant {id}.", HttpContext.TraceIdentifier));

        return NoContent();
    }
}
