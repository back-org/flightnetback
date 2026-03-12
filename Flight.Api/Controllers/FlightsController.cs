using Flight.Api.Models;
using Flight.Domain.Entities;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des vols.
/// Permet de consulter, créer, modifier et supprimer des vols.
/// </summary>
[Produces("application/json")]
public class FlightsController : ParentController
{
    private readonly Domain.Interfaces.IGenericRepository<Domain.Entities.Flight> _flightRepository;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des vols.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories.</param>
    public FlightsController(IRepositoryManager manager) : base(manager)
    {
        _flightRepository = Manager.Flight;
    }

    /// <summary>
    /// Retourne la liste complète des vols.
    /// </summary>
    /// <returns>Une collection de vols.</returns>
    [HttpGet]
    [EndpointName("GetAllFlights")]
    [EndpointSummary("Lister tous les vols")]
    [EndpointDescription("Retourne la liste complète des vols enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<Domain.Entities.Flight>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var flights = await _flightRepository.AllAsync();
        return Ok(flights);
    }

    /// <summary>
    /// Retourne le détail d'un vol à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique du vol.</param>
    /// <returns>Le vol correspondant si trouvé.</returns>
    [HttpGet("{id:int}")]
    [EndpointName("GetFlightById")]
    [EndpointSummary("Obtenir un vol par identifiant")]
    [EndpointDescription("Recherche un vol à partir de son identifiant. Retourne 404 si aucun vol correspondant n'existe.")]
    [ProducesResponseType(typeof(Domain.Entities.Flight), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Domain.Entities.Flight>> Get([FromRoute] int id)
    {
        var flight = await _flightRepository.GetByIdAsync(id);

        if (flight is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Vol introuvable.",
                Detail = $"Aucun vol n'a été trouvé avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(flight);
    }

    /// <summary>
    /// Crée un nouveau vol.
    /// </summary>
    /// <param name="flight">Données du vol à créer.</param>
    /// <returns>Le vol créé.</returns>
    [HttpPost]
    [EndpointName("CreateFlight")]
    [EndpointSummary("Créer un vol")]
    [EndpointDescription("Crée un nouveau vol à partir des données fournies dans le corps de la requête.")]
    [ProducesResponseType(typeof(Domain.Entities.Flight), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Domain.Entities.Flight>> Create([FromBody] FlightDto flight)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Le modèle envoyé est invalide.",
                Detail = "Vérifiez les champs obligatoires et les règles de validation.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            var entity = new Domain.Entities.Flight(flight);
            await _flightRepository.AddAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La création du vol a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Met à jour un vol existant.
    /// </summary>
    /// <param name="flight">Données du vol à mettre à jour.</param>
    /// <returns>Le vol mis à jour.</returns>
    [HttpPut]
    [EndpointName("UpdateFlight")]
    [EndpointSummary("Mettre à jour un vol")]
    [EndpointDescription("Met à jour un vol existant à partir des données fournies.")]
    [ProducesResponseType(typeof(Domain.Entities.Flight), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Domain.Entities.Flight>> Put([FromBody] FlightDto flight)
    {
        var item = await _flightRepository.GetByIdAsync(flight.Id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Vol introuvable.",
                Detail = $"Aucun vol n'a été trouvé avec l'identifiant {flight.Id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            item.Copy(flight);
            await _flightRepository.Update(item);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La mise à jour du vol a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item);
    }

    /// <summary>
    /// Supprime un vol à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant du vol à supprimer.</param>
    /// <returns>Le vol supprimé.</returns>
    [HttpDelete("{id:int}")]
    [EndpointName("DeleteFlight")]
    [EndpointSummary("Supprimer un vol")]
    [EndpointDescription("Supprime un vol existant. Retourne 404 si l'identifiant n'existe pas.")]
    [ProducesResponseType(typeof(Domain.Entities.Flight), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Domain.Entities.Flight>> Delete([FromRoute] int id)
    {
        var item = await _flightRepository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Vol introuvable.",
                Detail = $"Aucun vol n'a été trouvé avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            await _flightRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La suppression du vol a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item);
    }
}