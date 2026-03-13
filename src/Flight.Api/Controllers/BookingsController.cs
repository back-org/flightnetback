using Asp.Versioning;
using Flight.Api.Models;
using Flight.Application.CQRS.Commands.Bookings;
using Flight.Application.CQRS.Queries.Bookings;
using Flight.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des réservations.
/// Il permet de consulter, créer, modifier et supprimer des réservations.
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BookingsController : ParentController
{
    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des réservations.
    /// </summary>
    /// <param name="mediator">Médiateur chargé d'exécuter les commandes et requêtes.</param>
    public BookingsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllBookings")]
    [EndpointSummary("Lister toutes les réservations")]
    [EndpointDescription("Retourne la liste complète des réservations enregistrées dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<BookingDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllBookingsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetBookingById")]
    [EndpointSummary("Obtenir une réservation par identifiant")]
    [EndpointDescription("Recherche une réservation à partir de son identifiant. Retourne 404 si aucune réservation correspondante n'existe.")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingDto>> Get([FromRoute] int id)
    {
        var result = await Mediator.Send(new GetBookingByIdQuery(id));

        if (result is null)
        {
            return NotFoundResponse(
                "Réservation introuvable.",
                $"Aucune réservation n'a été trouvée avec l'identifiant {id}.");
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreateBooking")]
    [EndpointSummary("Créer une réservation")]
    [EndpointDescription("Crée une nouvelle réservation à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookingDto>> Create([FromBody] BookingDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new CreateBookingCommand(dto, User.Identity?.Name ?? "system"));

        return CreatedAtAction(nameof(Get), new { version = "1.0", id = result.Id }, result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdateBooking")]
    [EndpointSummary("Mettre à jour une réservation")]
    [EndpointDescription("Met à jour une réservation existante à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingDto>> Put([FromBody] BookingDto dto)
    {
        var validation = ValidateModel();
        if (validation is not null)
        {
            return validation;
        }

        var result = await Mediator.Send(
            new UpdateBookingCommand(dto.Id, dto, User.Identity?.Name ?? "system"));

        if (result is null)
        {
            return NotFoundResponse(
                "Réservation introuvable.",
                $"Aucune réservation n'a été trouvée avec l'identifiant {dto.Id}.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteBooking")]
    [EndpointSummary("Supprimer une réservation")]
    [EndpointDescription("Supprime définitivement une réservation existante. Endpoint réservé aux administrateurs.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var success = await Mediator.Send(
            new DeleteBookingCommand(id, User.Identity?.Name ?? "system"));

        if (!success)
        {
            return NotFoundResponse(
                "Réservation introuvable.",
                $"Aucune réservation n'a été trouvée avec l'identifiant {id}.");
        }

        return NoContent();
    }
}