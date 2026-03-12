using Flight.Api.Models;
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
/// <remarks>
/// Les opérations de lecture sont accessibles librement.
/// Les opérations d'écriture sont réservées aux utilisateurs ayant le rôle <c>Admin</c>.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AirlinesController : ParentController
{
    private readonly IGenericRepository<Airline> _airlineRepository;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des compagnies aériennes.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories injecté par l'application.</param>
    public AirlinesController(IRepositoryManager manager) : base(manager)
    {
        _airlineRepository = Manager.Airline;
    }

    /// <summary>
    /// Retourne la liste complète des compagnies aériennes enregistrées.
    /// </summary>
    /// <returns>Une collection de compagnies aériennes.</returns>
    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllAirlines")]
    [EndpointSummary("Lister toutes les compagnies aériennes")]
    [EndpointDescription("Retourne la liste complète des compagnies aériennes enregistrées dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<Airline>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var airlines = await _airlineRepository.AllAsync();
        return Ok(airlines);
    }

    /// <summary>
    /// Retourne le détail d'une compagnie aérienne à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique de la compagnie aérienne.</param>
    /// <returns>La compagnie aérienne correspondante si elle existe.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetAirlineById")]
    [EndpointSummary("Obtenir une compagnie aérienne par identifiant")]
    [EndpointDescription("Recherche une compagnie aérienne à partir de son identifiant. Retourne 404 si elle n'existe pas.")]
    [ProducesResponseType(typeof(Airline), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Airline>> Get([FromRoute] int id)
    {
        var airline = await _airlineRepository.GetByIdAsync(id);

        if (airline is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Compagnie aérienne introuvable.",
                Detail = $"Aucune compagnie aérienne n'a été trouvée avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(airline);
    }

    /// <summary>
    /// Crée une nouvelle compagnie aérienne.
    /// </summary>
    /// <param name="airline">Données de la compagnie aérienne à créer.</param>
    /// <returns>La compagnie créée avec son identifiant généré.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointName("CreateAirline")]
    [EndpointSummary("Créer une compagnie aérienne")]
    [EndpointDescription("Crée une nouvelle compagnie aérienne à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Airline), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Airline>> Create([FromBody] AirlineDto airline)
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
            var entity = new Airline(airline);
            await _airlineRepository.AddAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
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
    /// <param name="airline">Données mises à jour de la compagnie aérienne, incluant son identifiant.</param>
    /// <returns>La compagnie aérienne mise à jour.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [EndpointName("UpdateAirline")]
    [EndpointSummary("Mettre à jour une compagnie aérienne")]
    [EndpointDescription("Met à jour une compagnie aérienne existante à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Airline), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Airline>> Put([FromBody] AirlineDto airline)
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

        var item = await _airlineRepository.GetByIdAsync(airline.Id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Compagnie aérienne introuvable.",
                Detail = $"Aucune compagnie aérienne n'a été trouvée avec l'identifiant {airline.Id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            item.Copy(airline);
            await _airlineRepository.Update(item);

            return Ok(item);
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
    /// Supprime définitivement une compagnie aérienne à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique de la compagnie aérienne à supprimer.</param>
    /// <returns>Une réponse vide si la suppression réussit.</returns>
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
        var item = await _airlineRepository.GetByIdAsync(id);

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
            await _airlineRepository.DeleteAsync(id);
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