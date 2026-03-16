/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Countries' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Countries;

/// <summary>
/// Commande MediatR pour la suppression d'un pays.
/// </summary>
/// <param name="Id">Identifiant du pays à supprimer.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record DeleteCountryCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de traiter la suppression d'un pays.
/// </summary>
public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteCountryCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Country.GetByIdAsync(request.Id);

        if (existing is null)
        {
            return false;
        }

        await _manager.Country.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Country",
            entityId: request.Id.ToString(),
            details: $"Pays supprimé: {existing.Name}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}