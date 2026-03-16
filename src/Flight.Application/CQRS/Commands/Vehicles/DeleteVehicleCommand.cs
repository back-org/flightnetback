/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Vehicles' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Vehicles;

/// <summary>
/// Commande MediatR pour la suppression d'un véhicule.
/// </summary>
/// <param name="Id">Identifiant du véhicule à supprimer.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record DeleteVehicleCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de traiter la suppression d'un véhicule.
/// </summary>
public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteVehicleCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Vehicle.GetByIdAsync(request.Id);

        if (existing is null)
        {
            return false;
        }

        await _manager.Vehicle.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Vehicle",
            entityId: request.Id.ToString(),
            details: $"Véhicule supprimé: {existing.LicensePlate}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}