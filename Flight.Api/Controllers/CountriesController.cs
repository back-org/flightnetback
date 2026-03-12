using Flight.Api.Models;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur responsable de la gestion des pays.
/// Il permet de consulter, créer, modifier et supprimer des pays.
/// </summary>
/// <remarks>
/// Les opérations de lecture sont accessibles librement.
/// La création et la modification sont autorisées aux rôles <c>Admin</c> et <c>BasicUser</c>.
/// La suppression est réservée aux administrateurs.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CountriesController : ParentController
{
    private readonly IGenericRepository<Country> _repository;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur des pays.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories injecté par l'application.</param>
    public CountriesController(IRepositoryManager manager) : base(manager)
    {
        _repository = Manager.Country;
    }

    /// <summary>
    /// Retourne la liste complète des pays enregistrés.
    /// </summary>
    /// <returns>Une collection complète de pays.</returns>
    [HttpGet]
    [AllowAnonymous]
    [EndpointName("GetAllCountries")]
    [EndpointSummary("Lister tous les pays")]
    [EndpointDescription("Retourne la liste complète des pays enregistrés dans le système.")]
    [ProducesResponseType(typeof(IEnumerable<Country>), StatusCodes.Status200OK)]
    public override async Task<IActionResult> GetAll()
    {
        var items = await _repository.AllAsync();
        return Ok(items);
    }

    /// <summary>
    /// Retourne le détail d'un pays à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique du pays.</param>
    /// <returns>Le pays correspondant si trouvé.</returns>
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointName("GetCountryById")]
    [EndpointSummary("Obtenir un pays par identifiant")]
    [EndpointDescription("Recherche un pays à partir de son identifiant. Retourne 404 si aucun pays correspondant n'existe.")]
    [ProducesResponseType(typeof(Country), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Country>> Get([FromRoute] int id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound(new ErrorResponse
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Pays introuvable.",
                Detail = $"Aucun pays n'a été trouvé avec l'identifiant {id}.",
                TraceId = HttpContext.TraceIdentifier
            });
        }

        return Ok(item);
    }

    /// <summary>
    /// Crée un nouveau pays.
    /// </summary>
    /// <param name="dto">Données du pays à créer.</param>
    /// <returns>Le pays créé avec son identifiant généré.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("CreateCountry")]
    [EndpointSummary("Créer un pays")]
    [EndpointDescription("Crée un nouveau pays à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(Country), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Country>> Create([FromBody] CountryDto dto)
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
            var entity = new Country(dto);
            await _repository.AddAsync(entity);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "La création du pays a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Met à jour un pays existant.
    /// </summary>
    /// <param name="dto">Données mises à jour du pays, incluant son identifiant.</param>
    /// <returns>Le pays mis à jour.</returns>
    [HttpPut]
    [Authorize(Roles = "Admin,BasicUser")]
    [EndpointName("UpdateCountry")]
    [EndpointSummary("Mettre à jour un pays")]
    [EndpointDescription("Met à jour un pays existant à partir des données fournies. Endpoint autorisé aux rôles Admin et BasicUser.")]
    [ProducesResponseType(typeof(Country), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Country>> Put([FromBody] CountryDto dto)
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
                Message = "Pays introuvable.",
                Detail = $"Aucun pays n'a été trouvé avec l'identifiant {dto.Id}.",
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
                Message = "La mise à jour du pays a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }

    /// <summary>
    /// Supprime définitivement un pays à partir de son identifiant.
    /// </summary>
    /// <param name="id">Identifiant unique du pays à supprimer.</param>
    /// <returns>Une réponse vide si la suppression réussit.</returns>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [EndpointName("DeleteCountry")]
    [EndpointSummary("Supprimer un pays")]
    [EndpointDescription("Supprime définitivement un pays existant. Endpoint réservé aux administrateurs.")]
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
                Message = "Pays introuvable.",
                Detail = $"Aucun pays n'a été trouvé avec l'identifiant {id}.",
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
                Message = "La suppression du pays a échoué.",
                Detail = ex.InnerException?.Message ?? ex.Message,
                TraceId = HttpContext.TraceIdentifier
            });
        }
    }
}