using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur gérant les opérations CRUD sur les aéroports (<see cref="Airport"/>).
/// Accès en lecture libre, opérations d'écriture réservées aux administrateurs.
/// </summary>
public class AirportsController : ParentController
{
    private readonly IGenericRepository<Airport> _airportRepository;

    /// <summary>
    /// Initialise une nouvelle instance du <see cref="AirportsController"/>.
    /// </summary>
    /// <param name="manager">Le gestionnaire de dépôts injecté par DI.</param>
    public AirportsController(IRepositoryManager manager) : base(manager)
    {
        _airportRepository = Manager.Airport;
    }

    /// <summary>
    /// Retourne la liste complète de tous les aéroports enregistrés.
    /// </summary>
    /// <returns>Une liste de <see cref="Airport"/>.</returns>
    [EndpointDescription("Retourne la liste de tous les aéroports.")]
    [ProducesResponseType(typeof(IEnumerable<Airport>), StatusCodes.Status200OK)]
    [EndpointName("GetAllAirports")]
    [EndpointSummary("Tous les aéroports")]
    public override async Task<IActionResult> GetAll()
    {
        var airports = await _airportRepository.AllAsync();
        return Ok(airports);
    }

    /// <summary>
    /// Récupère un aéroport par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'aéroport.</param>
    /// <returns>L'aéroport correspondant, ou 404 si non trouvé.</returns>
    [HttpGet("{id:int}")]
    [EndpointName("GetAirportById")]
    [EndpointSummary("Aéroport par ID")]
    [EndpointDescription("Retourne un aéroport par son ID, ou 404 s'il n'existe pas.")]
    [ProducesResponseType(typeof(Airport), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Airport>> Get(int id)
    {
        var airport = await _airportRepository.GetByIdAsync(id);
        if (airport == null) return NotFound(new { message = $"Aéroport avec l'ID {id} non trouvé." });
        return Ok(airport);
    }

    /// <summary>
    /// Crée un nouvel aéroport.
    /// </summary>
    /// <param name="airport">Les données de l'aéroport à créer.</param>
    /// <returns>L'aéroport créé avec son nouvel identifiant.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Créer un aéroport")]
    [EndpointDescription("Crée un nouvel aéroport. Réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Airport), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Airport>> Create([FromBody] AirportDto airport)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var entity = new Airport(airport);
            await _airportRepository.AddAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Met à jour un aéroport existant.
    /// </summary>
    /// <param name="airport">Les nouvelles données de l'aéroport (doit inclure l'ID).</param>
    /// <returns>L'aéroport mis à jour, ou 400/404 en cas d'erreur.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Mettre à jour un aéroport")]
    [EndpointDescription("Met à jour un aéroport existant. Réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Airport), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Airport>> Put([FromBody] AirportDto airport)
    {
        var item = await _airportRepository.GetByIdAsync(airport.Id);
        if (item is null) return NotFound(new { message = $"Aéroport avec l'ID {airport.Id} non trouvé." });

        try
        {
            item.Copy(airport);
            await _airportRepository.Update(item);
            return Ok(item);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Supprime un aéroport par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de l'aéroport à supprimer.</param>
    /// <returns>204 No Content si supprimé, 404 si non trouvé.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer un aéroport")]
    [EndpointDescription("Supprime définitivement un aéroport. Réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var item = await _airportRepository.GetByIdAsync(id);
        if (item is null) return NotFound(new { message = $"Aéroport avec l'ID {id} non trouvé." });

        try
        {
            await _airportRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }
}
