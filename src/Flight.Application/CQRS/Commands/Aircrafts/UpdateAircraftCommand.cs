/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Aircrafts' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Aircrafts;

/// <summary>
/// Commande permettant de mettre à jour un avion existant.
/// </summary>
public record UpdateAircraftCommand(int Id, AircraftDto Dto, string PerformedBy) : IRequest<AircraftDto?>;

/// <summary>
/// Handler chargé de mettre à jour un avion.
/// </summary>
public class UpdateAircraftCommandHandler : IRequestHandler<UpdateAircraftCommand, AircraftDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateAircraftCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<AircraftDto?> Handle(UpdateAircraftCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Aircraft.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Aircraft.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Aircraft",
            entityId: existing.Id.ToString(),
            details: $"Avion mis à jour : {existing.RegistrationNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}