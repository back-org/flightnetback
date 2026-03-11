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
/// Contrôleur gérant les opérations CRUD sur les entités <see cref="Passenger"/>.
/// </summary>
public class PassengersController : ParentController
{
    private readonly IGenericRepository<Passenger> _repository;

    /// <summary>
    /// Initialise une nouvelle instance du <see cref="PassengersController"/>.
    /// </summary>
    /// <param name="manager">Le gestionnaire de dépôts injecté par DI.</param>
    public PassengersController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.Passenger;
    }

    /// <summary>
    /// Retourne la liste complète des <see cref="Passenger"/> enregistrés.
    /// </summary>
    /// <returns>Une liste de <see cref="Passenger"/>.</returns>
    [ProducesResponseType(typeof(IEnumerable<Passenger>), StatusCodes.Status200OK)]
    [EndpointName("GetAllPassengers")]
    [EndpointSummary("Tous les passengers")]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items);
    }

    /// <summary>
    /// Récupère un(e) <see cref="Passenger"/> par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de la ressource.</param>
    /// <returns>La ressource correspondante, ou 404 si non trouvée.</returns>
    [HttpGet("{id:int}")]
    [EndpointName("GetPassengerById")]
    [EndpointSummary("Passenger par ID")]
    [ProducesResponseType(typeof(Passenger), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Passenger>> Get(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null) return NotFound(new { message = $"Passenger avec l'ID {id} non trouvé(e)." });
        return Ok(item);
    }

    /// <summary>
    /// Crée un(e) nouveau/nouvelle <see cref="Passenger"/>.
    /// </summary>
    /// <param name="dto">Les données de la ressource à créer.</param>
    /// <returns>La ressource créée avec son nouvel identifiant.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointSummary("Créer un(e) passenger")]
    [ProducesResponseType(typeof(Passenger), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Passenger>> Create([FromBody] PassengerDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var entity = new Passenger(dto);
            await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Met à jour un(e) <see cref="Passenger"/> existant(e).
    /// </summary>
    /// <param name="dto">Les nouvelles données (doit inclure l'ID).</param>
    /// <returns>La ressource mise à jour, ou 400/404 en cas d'erreur.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointSummary("Mettre à jour un(e) passenger")]
    [ProducesResponseType(typeof(Passenger), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Passenger>> Put([FromBody] PassengerDto dto)
    {
        var item = await _repository.GetByIdAsync(dto.Id);
        if (item is null) return NotFound(new { message = $"Passenger avec l'ID {dto.Id} non trouvé(e)." });

        try
        {
            item.Copy(dto);
            await _repository.Update(item);
            return Ok(item);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Supprime un(e) <see cref="Passenger"/> par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de la ressource à supprimer.</param>
    /// <returns>204 No Content si supprimé(e), 404 si non trouvé(e).</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer un(e) passenger")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null) return NotFound(new { message = $"Passenger avec l'ID {id} non trouvé(e)." });

        try
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }
}
