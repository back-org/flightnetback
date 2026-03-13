using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur de base utilisé par les contrôleurs métier.
/// Il centralise l'accès au gestionnaire de repositories partagé par l'application.
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class ParentController : ControllerBase
{
    /// <summary>
    /// Obtient le gestionnaire central des repositories métier.
    /// </summary>
    protected IRepositoryManager Manager { get; }

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur parent.
    /// </summary>
    /// <param name="manager">Gestionnaire de repositories injecté par le conteneur de dépendances.</param>
    protected ParentController(IRepositoryManager manager)
    {
        Manager = manager;
    }

    /// <summary>
    /// Retourne la liste complète des éléments gérés par le contrôleur enfant.
    /// Cette méthode est redéfinie dans chaque contrôleur spécialisé.
    /// </summary>
    /// <returns>Une réponse HTTP contenant la collection demandée.</returns>
    [HttpGet]
    public virtual Task<IActionResult> GetAll()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
}