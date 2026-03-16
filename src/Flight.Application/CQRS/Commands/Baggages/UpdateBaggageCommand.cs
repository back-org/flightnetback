/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Baggages' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Baggages;

/// <summary>
/// Commande permettant de mettre à jour un bagage existant.
/// </summary>
public record UpdateBaggageCommand(int Id, BaggageDto Dto, string PerformedBy) : IRequest<BaggageDto?>;

/// <summary>
/// Handler chargé de mettre à jour un bagage.
/// </summary>
public class UpdateBaggageCommandHandler : IRequestHandler<UpdateBaggageCommand, BaggageDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdateBaggageCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<BaggageDto?> Handle(UpdateBaggageCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Baggage.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Baggage.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Baggage",
            entityId: existing.Id.ToString(),
            details: $"Bagage mis à jour : {existing.TagNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}