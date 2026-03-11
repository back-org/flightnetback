using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur gérant les opérations CRUD sur les vols (<see cref="Flight.Domain.Entities.Flight"/>).
/// Accès en lecture libre, opérations d'écriture réservées aux utilisateurs authentifiés.
/// </summary>
public class FlightsController : ParentController
{
    private readonly IGenericRepository<Domain.Entities.Flight> _flightRepository;

    /// <summary>
    /// Initialise une nouvelle instance du <see cref="FlightsController"/>.
    /// </summary>
    /// <param name="manager">Le gestionnaire de dépôts injecté par DI.</param>
    public FlightsController(IRepositoryManager manager) : base(manager)
    {
        _flightRepository = Manager.Flight;
    }

    /// <summary>
    /// Retourne la liste complète de tous les vols enregistrés dans le système.
    /// </summary>
    /// <returns>Une liste de <see cref="Domain.Entities.Flight"/>.</returns>
    [EndpointDescription("Retourne la liste de tous les vols disponibles.")]
    [ProducesResponseType(typeof(IEnumerable<Domain.Entities.Flight>), StatusCodes.Status200OK)]
    [EndpointName("GetAllFlights")]
    [EndpointSummary("Tous les vols")]
    public override async Task<IActionResult> GetAll()
    {
        var flights = await _flightRepository.AllAsync();
        return Ok(flights);
    }

    /// <summary>
    /// Récupère un vol par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du vol.</param>
    /// <returns>Le vol correspondant, ou 404 si non trouvé.</returns>
    [HttpGet("{id:int}")]
    [EndpointName("GetFlightById")]
    [EndpointSummary("Vol par ID")]
    [EndpointDescription("Retourne un vol par son ID, ou 404 s'il n'existe pas.")]
    [ProducesResponseType(typeof(Domain.Entities.Flight), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Domain.Entities.Flight>> Get(int id)
    {
        var flight = await _flightRepository.GetByIdAsync(id);
        if (flight == null) return NotFound(new { message = $"Vol avec l'ID {id} non trouvé." });
        return Ok(flight);
    }

    /// <summary>
    /// Crée un nouveau vol.
    /// </summary>
    /// <param name="flight">Les données du vol à créer.</param>
    /// <returns>Le vol créé avec son nouvel identifiant.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Créer un vol")]
    [EndpointDescription("Crée un nouveau vol. Réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Domain.Entities.Flight), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Domain.Entities.Flight>> Create([FromBody] Domain.Entities.FlightDto flight)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var entity = new Domain.Entities.Flight(flight);
            await _flightRepository.AddAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Met à jour un vol existant.
    /// </summary>
    /// <param name="flight">Les nouvelles données du vol (doit inclure l'ID).</param>
    /// <returns>Le vol mis à jour, ou 400/404 en cas d'erreur.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Mettre à jour un vol")]
    [EndpointDescription("Met à jour un vol existant. Réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Domain.Entities.Flight), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Domain.Entities.Flight>> Put([FromBody] Domain.Entities.FlightDto flight)
    {
        var item = await _flightRepository.GetByIdAsync(flight.Id);
        if (item is null) return NotFound(new { message = $"Vol avec l'ID {flight.Id} non trouvé." });

        try
        {
            item.Copy(flight);
            await _flightRepository.Update(item);
            return Ok(item);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Supprime un vol par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant du vol à supprimer.</param>
    /// <returns>204 No Content si supprimé, 404 si non trouvé.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer un vol")]
    [EndpointDescription("Supprime définitivement un vol. Réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var item = await _flightRepository.GetByIdAsync(id);
        if (item is null) return NotFound(new { message = $"Vol avec l'ID {id} non trouvé." });

        try
        {
            await _flightRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }
}
