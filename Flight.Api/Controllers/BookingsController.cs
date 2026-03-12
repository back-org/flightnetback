using Flight.Api.Models;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des réservations.
/// Il permet de consulter, créer, modifier et supprimer des réservations.
/// </summary>
/// <remarks>
/// Les opérations de lecture sont accessibles librement.
/// La création et la modification sont autorisées aux rôles <c>Admin</c> et <c>BasicUser</c>.
/// La suppression est réservée aux administrateurs.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BookingsController : ParentController
{
    private readonly IGenericRepository<Booking> _repository;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des réservations.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories injecté par l'application.</param>
    public BookingsController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.Booking;
    }

    /// <summary>
    /// Retourne la liste complète des réservations enregistrées.
    /// </summary>
    /// <returns>Une collection complète de réservations.</returns>
    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllBookings")]
    [EndpointSummary("Lister toutes les réservations")]
    [EndpointDescription("Retourne la liste complète des réservations enregistrées dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<Booking>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items);
    }

    /// <summary>
    /// Retourne le détail d'une réservation à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique de la réservation.</param>
    /// <returns>La réservation correspondante si elle existe.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetBookingById")]
    [EndpointSummary("Obtenir une réservation par identifiant")]
    [EndpointDescription("Recherche une réservation à partir de son identifiant. Retourne 404 si aucune réservation correspondante n'existe.")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> Get([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Réservation introuvable.",
                Detail = $"Aucune réservation n'a été trouvée avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item);
    }

    /// <summary>
    /// Crée une nouvelle réservation.
    /// </summary>
    /// <param name="dto">Données de la réservation à créer.</param>
    /// <returns>La réservation créée avec son identifiant généré.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreateBooking")]
    [EndpointSummary("Créer une réservation")]
    [EndpointDescription("Crée une nouvelle réservation à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Booking>> Create([FromBody] BookingDto dto)
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
            var entity = new Booking(dto);
            await _repository.AddAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La création de la réservation a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Met à jour une réservation existante.
    /// </summary>
    /// <param name="dto">Données mises à jour de la réservation, incluant son identifiant.</param>
    /// <returns>La réservation mise à jour.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdateBooking")]
    [EndpointSummary("Mettre à jour une réservation")]
    [EndpointDescription("Met à jour une réservation existante à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Booking>> Put([FromBody] BookingDto dto)
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

        var item = await _repository.GetByIdAsync(dto.Id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Réservation introuvable.",
                Detail = $"Aucune réservation n'a été trouvée avec l'identifiant {dto.Id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            item.Copy(dto);
            await _repository.Update(item);

            return Ok(item);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La mise à jour de la réservation a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Supprime définitivement une réservation à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique de la réservation à supprimer.</param>
    /// <returns>Une réponse vide si la suppression réussit.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteBooking")]
    [EndpointSummary("Supprimer une réservation")]
    [EndpointDescription("Supprime définitivement une réservation existante. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Réservation introuvable.",
                Detail = $"Aucune réservation n'a été trouvée avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        try
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La suppression de la réservation a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }
}