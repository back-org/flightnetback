using Flight.Api.Models;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des villes.
/// Il permet de consulter, créer, modifier et supprimer des villes.
/// </summary>
/// <remarks>
/// Les opérations de lecture sont accessibles librement.
/// La création et la modification sont autorisées aux rôles <c>Admin</c> et <c>BasicUser</c>.
/// La suppression est réservée aux administrateurs.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CitiesController : ParentController
{
    private readonly IGenericRepository<City> _repository;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des villes.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories injecté par l'application.</param>
    public CitiesController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.City;
    }

    /// <summary>
    /// Retourne la liste complète des villes enregistrées.
    /// </summary>
    /// <returns>Une collection complète de villes.</returns>
    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllCities")]
    [EndpointSummary("Lister toutes les villes")]
    [EndpointDescription("Retourne la liste complète des villes enregistrées dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<City>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items);
    }

    /// <summary>
    /// Retourne le détail d'une ville à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique de la ville.</param>
    /// <returns>La ville correspondante si elle existe.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetCityById")]
    [EndpointSummary("Obtenir une ville par identifiant")]
    [EndpointDescription("Recherche une ville à partir de son identifiant. Retourne 404 si aucune ville correspondante n'existe.")]
    [ProducesResponseType(typeof(City), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<City>> Get([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Ville introuvable.",
                Detail = $"Aucune ville n'a été trouvée avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item);
    }

    /// <summary>
    /// Crée une nouvelle ville.
    /// </summary>
    /// <param name="dto">Données de la ville à créer.</param>
    /// <returns>La ville créée avec son identifiant généré.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreateCity")]
    [EndpointSummary("Créer une ville")]
    [EndpointDescription("Crée une nouvelle ville à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(City), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<City>> Create([FromBody] CityDto dto)
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
            var entity = new City(dto);
            await _repository.AddAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La création de la ville a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Met à jour une ville existante.
    /// </summary>
    /// <param name="dto">Données mises à jour de la ville, incluant son identifiant.</param>
    /// <returns>La ville mise à jour.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdateCity")]
    [EndpointSummary("Mettre à jour une ville")]
    [EndpointDescription("Met à jour une ville existante à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(City), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<City>> Put([FromBody] CityDto dto)
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
                Message = "Ville introuvable.",
                Detail = $"Aucune ville n'a été trouvée avec l'identifiant {dto.Id}.",
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
                Message = "La mise à jour de la ville a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Supprime définitivement une ville à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique de la ville à supprimer.</param>
    /// <returns>Une réponse vide si la suppression réussit.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteCity")]
    [EndpointSummary("Supprimer une ville")]
    [EndpointDescription("Supprime définitivement une ville existante. Endpoint réservé aux administrateurs.")]
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
                Message = "Ville introuvable.",
                Detail = $"Aucune ville n'a été trouvée avec l'identifiant {id}.",
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
                Message = "La suppression de la ville a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }
}