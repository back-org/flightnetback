using Flight.Api.Models;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des aéroports.
/// Il permet de consulter, créer, modifier et supprimer des aéroports.
/// </summary>
/// <remarks>
/// Les opérations de lecture sont accessibles librement.
/// Les opérations d'écriture sont réservées aux utilisateurs ayant le rôle <c>Admin</c>.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AirportsController : ParentController
{
    private readonly IGenericRepository<Airport> _airportRepository;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des aéroports.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories injecté par l'application.</param>
    public AirportsController(IRepositoryManager manager) : base(manager)
    {
        _airportRepository = Manager.Airport;
    }

    /// <summary>
    /// Retourne la liste complète des aéroports enregistrés.
    /// </summary>
    /// <returns>Une collection de tous les aéroports disponibles.</returns>
    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllAirports")]
    [EndpointSummary("Lister tous les aéroports")]
    [EndpointDescription("Retourne la liste complète des aéroports enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<Airport>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var airports = await _airportRepository.AllAsync();
        return Ok(airports);
    }

    /// <summary>
    /// Retourne le détail d'un aéroport à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique de l'aéroport.</param>
    /// <returns>L'aéroport correspondant si trouvé.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetAirportById")]
    [EndpointSummary("Obtenir un aéroport par identifiant")]
    [EndpointDescription("Recherche un aéroport à partir de son identifiant. Retourne 404 si aucun aéroport correspondant n'existe.")]
    [ProducesResponseType(typeof(Airport), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Airport>> Get([FromRoute] int id)
    {
        var airport = await _airportRepository.GetByIdAsync(id);

        if (airport is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Aéroport introuvable.",
                Detail = $"Aucun aéroport n'a été trouvé avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(airport);
    }

    /// <summary>
    /// Crée un nouvel aéroport.
    /// </summary>
    /// <param name="airport">Données de l'aéroport à créer.</param>
    /// <returns>L'aéroport créé avec son identifiant généré.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointName("CreateAirport")]
    [EndpointSummary("Créer un aéroport")]
    [EndpointDescription("Crée un nouvel aéroport à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Airport), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Airport>> Create([FromBody] AirportDto airport)
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
            var entity = new Airport(airport);
            await _airportRepository.AddAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
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

    /// <summary>
    /// Met à jour un aéroport existant.
    /// </summary>
    /// <param name="airport">Données mises à jour de l'aéroport, incluant son identifiant.</param>
    /// <returns>L'aéroport mis à jour.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [EndpointName("UpdateAirport")]
    [EndpointSummary("Mettre à jour un aéroport")]
    [EndpointDescription("Met à jour un aéroport existant à partir des données fournies. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(typeof(Airport), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Airport>> Put([FromBody] AirportDto airport)
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

        var item = await _airportRepository.GetByIdAsync(airport.Id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Aéroport introuvable.",
                Detail = $"Aucun aéroport n'a été trouvé avec l'identifiant {airport.Id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            item.Copy(airport);
            await _airportRepository.Update(item);

            return Ok(item);
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

    /// <summary>
    /// Supprime définitivement un aéroport à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique de l'aéroport à supprimer.</param>
    /// <returns>Une réponse vide si la suppression réussit.</returns>
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
        var item = await _airportRepository.GetByIdAsync(id);

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
            await _airportRepository.DeleteAsync(id);
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