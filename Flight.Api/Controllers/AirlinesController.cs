using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur gérant les opérations CRUD sur les compagnies aériennes (<see cref="Airline"/>).
/// Accès en lecture libre, opérations d'écriture réservées aux administrateurs.
/// </summary>
public class AirlinesController : ParentController
{
    private readonly IGenericRepository<Airline> _airlineRepository;

    /// <summary>
    /// Initialise une nouvelle instance du <see cref="AirlinesController"/>.
    /// </summary>
    /// <param name="manager">Le gestionnaire de dépôts injecté par DI.</param>
    public AirlinesController(IRepositoryManager manager) : base(manager)
    {
        _airlineRepository = Manager.Airline;
    }

    /// <summary>
    /// Retourne la liste complète de toutes les compagnies aériennes enregistrées.
    /// </summary>
    /// <returns>Une liste de <see cref="Airline"/>.</returns>
    [EndpointDescription("Retourne la liste de toutes les compagnies aériennes.")]
    [ProducesResponseType(typeof(IEnumerable<Airline>), StatusCodes.Status200OK)]
    [EndpointName("GetAllAirlines")]
    [EndpointSummary("Toutes les compagnies aériennes")]
    public override async Task<IActionResult> GetAll()
    {
        var airlines = await _airlineRepository.AllAsync();
        return Ok(airlines);
    }

    /// <summary>
    /// Récupère une compagnie aérienne par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de la compagnie aérienne.</param>
    /// <returns>La compagnie aérienne correspondante, ou 404 si non trouvée.</returns>
    [HttpGet("{id:int}")]
    [EndpointName("GetAirlineById")]
    [EndpointSummary("Compagnie aérienne par ID")]
    [EndpointDescription("Retourne une compagnie aérienne par son ID, ou 404 si elle n'existe pas.")]
    [ProducesResponseType(typeof(Airline), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Airline>> Get(int id)
    {
        var airline = await _airlineRepository.GetByIdAsync(id);
        if (airline == null) return NotFound(new { message = $"Compagnie aérienne avec l'ID {id} non trouvée." });
        return Ok(airline);
    }

    /// <summary>
    /// Crée une nouvelle compagnie aérienne.
    /// </summary>
    /// <param name="airline">Les données de la compagnie à créer.</param>
    /// <returns>La compagnie créée avec son nouvel identifiant.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Créer une compagnie aérienne")]
    [EndpointDescription("Crée une nouvelle compagnie aérienne. Réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Airline), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Airline>> Create([FromBody] AirlineDto airline)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var entity = new Airline(airline);
            await _airlineRepository.AddAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Met à jour une compagnie aérienne existante.
    /// </summary>
    /// <param name="airline">Les nouvelles données de la compagnie (doit inclure l'ID).</param>
    /// <returns>La compagnie mise à jour, ou 400 en cas d'erreur.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Mettre à jour une compagnie aérienne")]
    [EndpointDescription("Met à jour une compagnie aérienne existante. Réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Airline), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Airline>> Put([FromBody] AirlineDto airline)
    {
        var item = await _airlineRepository.GetByIdAsync(airline.Id);
        if (item is null) return NotFound(new { message = $"Compagnie aérienne avec l'ID {airline.Id} non trouvée." });

        try
        {
            item.Copy(airline);
            await _airlineRepository.Update(item);
            return Ok(item);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }

    /// <summary>
    /// Supprime une compagnie aérienne par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de la compagnie à supprimer.</param>
    /// <returns>204 No Content si supprimée, 404 si non trouvée.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer une compagnie aérienne")]
    [EndpointDescription("Supprime définitivement une compagnie aérienne. Réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var item = await _airlineRepository.GetByIdAsync(id);
        if (item is null) return NotFound(new { message = $"Compagnie aérienne avec l'ID {id} non trouvée." });

        try
        {
            await _airlineRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.InnerException?.Message ?? e.Message });
        }
    }
}
