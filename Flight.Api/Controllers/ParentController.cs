using System.Threading.Tasks;
using Flight.Domain.Core.Abstracts;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Api.Controllers;

/// <summary>
/// Contrôleur de base dont héritent tous les contrôleurs de ressources.
/// Fournit la méthode <see cref="GetAll"/> surchargeable et l'accès au <see cref="IRepositoryManager"/>.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ParentController : ControllerBase
{
    /// <summary>
    /// Référence protégée vers un dépôt générique (non utilisée directement dans cette classe).
    /// </summary>
    protected IGenericRepository<DeleteEntity<int>>? _repository { get; set; }

    /// <summary>
    /// Gestionnaire de dépôts permettant d'accéder à toutes les entités.
    /// </summary>
    protected IRepositoryManager Manager { get; }

    /// <summary>
    /// Initialise une nouvelle instance du <see cref="ParentController"/>.
    /// </summary>
    /// <param name="manager">Le gestionnaire de dépôts injecté par DI.</param>
    public ParentController(IRepositoryManager manager)
    {
        Manager = manager;
    }

    /// <summary>
    /// Retourne la liste complète des ressources. 
    /// Cette méthode doit être surchargée par les contrôleurs enfants.
    /// </summary>
    /// <returns>
    /// Un <see cref="IActionResult"/> contenant la liste des entités, ou <c>null</c> si non surchargée.
    /// </returns>
    [HttpGet]
    public virtual Task<IActionResult> GetAll()
    {
        return Task.FromResult<IActionResult>(Ok());
    }
}
