/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Vehicles' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Vehicles;

/// <summary>
/// Commande MediatR pour la mise à jour d'un véhicule.
/// </summary>
/// <param name="Id">Identifiant du véhicule à mettre à jour.</param>
/// <param name="Dto">Données mises à jour.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record UpdateVehicleCommand(int Id, VehicleDto Dto, string PerformedBy) : IRequest<VehicleDto?>;

/// <summary>
/// Handler chargé de traiter la mise à jour d'un véhicule.
/// </summary>
public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, VehicleDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateVehicleCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<VehicleDto?> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Vehicle.GetByIdAsync(request.Id);

        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Vehicle.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Vehicle",
            entityId: existing.Id.ToString(),
            details: $"Véhicule mis à jour: {existing.LicensePlate}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}