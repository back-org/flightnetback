using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur gérant les opérations CRUD sur les entités <see cref="Booking"/>.
/// </summary>
public class BookingsController : ParentController
{
    private readonly IGenericRepository<Booking> _repository;

    /// <summary>
    /// Initialise une nouvelle instance du <see cref="BookingsController"/>.
    /// </summary>
    /// <param name="manager">Le gestionnaire de dépôts injecté par DI.</param>
    public BookingsController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.Booking;
    }

    /// <summary>
    /// Retourne la liste complète des <see cref="Booking"/> enregistrés.
    /// </summary>
    /// <returns>Une liste de <see cref="Booking"/>.</returns>
    [ProducesResponseType(typeof(IEnumerable<Booking>), StatusCodes.Status200OK)]
    [EndpointName("GetAllBookings")]
    [EndpointSummary("Tous les bookings")]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items);
    }

    /// <summary>
    /// Récupère un(e) <see cref="Booking"/> par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de la ressource.</param>
    /// <returns>La ressource correspondante, ou 404 si non trouvée.</returns>
    [HttpGet("{id:int}")]
    [EndpointName("GetBookingById")]
    [EndpointSummary("Booking par ID")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> Get(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item == null) return NotFound(new { message = $"Booking avec l'ID {id} non trouvé(e)." });
        return Ok(item);
    }

    /// <summary>
    /// Crée un(e) nouveau/nouvelle <see cref="Booking"/>.
    /// </summary>
    /// <param name="dto">Les données de la ressource à créer.</param>
    /// <returns>La ressource créée avec son nouvel identifiant.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointSummary("Créer un(e) booking")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Booking>> Create([FromBody] BookingDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var entity = new Booking(dto);
            await _repository.AddAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Met à jour un(e) <see cref="Booking"/> existant(e).
    /// </summary>
    /// <param name="dto">Les nouvelles données (doit inclure l'ID).</param>
    /// <returns>La ressource mise à jour, ou 400/404 en cas d'erreur.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointSummary("Mettre à jour un(e) booking")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> Put([FromBody] BookingDto dto)
    {
        var item = await _repository.GetByIdAsync(dto.Id);
        if (item is null) return NotFound(new { message = $"Booking avec l'ID {dto.Id} non trouvé(e)." });

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
    /// Supprime un(e) <see cref="Booking"/> par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de la ressource à supprimer.</param>
    /// <returns>204 No Content si supprimé(e), 404 si non trouvé(e).</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer un(e) booking")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null) return NotFound(new { message = $"Booking avec l'ID {id} non trouvé(e)." });

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
