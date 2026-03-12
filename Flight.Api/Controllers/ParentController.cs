using System.Threading.Tasks;
using Flight.Domain.Core.Abstracts;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur de base utilisé par les contrôleurs métier.
/// Il centralise l'accès au gestionnaire de repositories.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ParentController : ControllerBase
{
    /// <summary>
    /// Repository générique de base.
    /// </summary>
    protected IGenericRepository<DeleteEntity<int>> _repository { get; set; }

    /// <summary>
    /// Gestionnaire central des repositories métier.
    /// </summary>
    protected IRepositoryManager Manager { get; }

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur parent.
    /// </summary>
    /// <param name="manager">Gestionnaire de repositories injecté.</param>
    public ParentController(IRepositoryManager manager)
    {
        Manager = manager;
    }

    /// <summary>
    /// Retourne la liste complète des éléments.
    /// Cette méthode est destinée à être redéfinie dans les contrôleurs enfants.
    /// </summary>
    /// <returns>Une réponse HTTP contenant la collection demandée.</returns>
    [HttpGet]
    public virtual Task<IActionResult> GetAll()
    {
        return Task.FromResult<IActionResult>(null!);
    }
}