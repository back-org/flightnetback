/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Vehicles' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Vehicles;

/// <summary>
/// Commande MediatR pour la création d'un véhicule.
/// </summary>
/// <param name="Dto">Données du véhicule à créer.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record CreateVehicleCommand(VehicleDto Dto, string PerformedBy) : IRequest<VehicleDto>;

/// <summary>
/// Handler chargé de traiter la création d'un véhicule.
/// </summary>
public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, VehicleDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateVehicleCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<VehicleDto> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.Vehicle.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "Vehicle",
            entityId: entity.Id.ToString(),
            details: $"Véhicule créé: {entity.LicensePlate}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}