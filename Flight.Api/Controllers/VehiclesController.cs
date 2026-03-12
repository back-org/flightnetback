using Flight.Api.Models;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des véhicules.
/// Il permet de consulter, créer, modifier et supprimer des véhicules.
/// </summary>
/// <remarks>
/// Les opérations de lecture sont accessibles librement.
/// La création et la modification sont autorisées aux rôles <c>Admin</c> et <c>BasicUser</c>.
/// La suppression est réservée aux administrateurs.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class VehiclesController : ParentController
{
    private readonly IGenericRepository<Vehicle> _repository;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des véhicules.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories injecté par l'application.</param>
    public VehiclesController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.Vehicle;
    }

    /// <summary>
    /// Retourne la liste complète des véhicules enregistrés.
    /// </summary>
    /// <returns>Une collection complète de véhicules.</returns>
    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllVehicles")]
    [EndpointSummary("Lister tous les véhicules")]
    [EndpointDescription("Retourne la liste complète des véhicules enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<Vehicle>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items);
    }

    /// <summary>
    /// Retourne le détail d'un véhicule à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique du véhicule.</param>
    /// <returns>Le véhicule correspondant si trouvé.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetVehicleById")]
    [EndpointSummary("Obtenir un véhicule par identifiant")]
    [EndpointDescription("Recherche un véhicule à partir de son identifiant. Retourne 404 si aucun véhicule correspondant n'existe.")]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Vehicle>> Get([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Véhicule introuvable.",
                Detail = $"Aucun véhicule n'a été trouvé avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item);
    }

    /// <summary>
    /// Crée un nouveau véhicule.
    /// </summary>
    /// <param name="dto">Données du véhicule à créer.</param>
    /// <returns>Le véhicule créé avec son identifiant généré.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreateVehicle")]
    [EndpointSummary("Créer un véhicule")]
    [EndpointDescription("Crée un nouveau véhicule à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Vehicle>> Create([FromBody] VehicleDto dto)
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
            var entity = new Vehicle(dto);
            await _repository.AddAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La création du véhicule a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Met à jour un véhicule existant.
    /// </summary>
    /// <param name="dto">Données mises à jour du véhicule, incluant son identifiant.</param>
    /// <returns>Le véhicule mis à jour.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdateVehicle")]
    [EndpointSummary("Mettre à jour un véhicule")]
    [EndpointDescription("Met à jour un véhicule existant à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(Vehicle), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Vehicle>> Put([FromBody] VehicleDto dto)
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
                Message = "Véhicule introuvable.",
                Detail = $"Aucun véhicule n'a été trouvé avec l'identifiant {dto.Id}.",
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
                Message = "La mise à jour du véhicule a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Supprime définitivement un véhicule à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique du véhicule à supprimer.</param>
    /// <returns>Une réponse vide si la suppression réussit.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteVehicle")]
    [EndpointSummary("Supprimer un véhicule")]
    [EndpointDescription("Supprime définitivement un véhicule existant. Endpoint réservé aux administrateurs.")]
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
                Message = "Véhicule introuvable.",
                Detail = $"Aucun véhicule n'a été trouvé avec l'identifiant {id}.",
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
                Message = "La suppression du véhicule a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }
}