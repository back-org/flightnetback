/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Passengers' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Passengers;

/// <summary>
/// Commande MediatR pour la mise à jour d'un passager.
/// </summary>
/// <param name="Id">Identifiant du passager à mettre à jour.</param>
/// <param name="Dto">Données mises à jour.</param>
/// <param name="PerformedBy">Utilisateur ayant déclenché l'action.</param>
public record UpdatePassengerCommand(int Id, PassengerDto Dto, string PerformedBy) : IRequest<PassengerDto?>;

/// <summary>
/// Handler chargé de traiter la mise à jour d'un passager.
/// </summary>
public class UpdatePassengerCommandHandler : IRequestHandler<UpdatePassengerCommand, PassengerDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public UpdatePassengerCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<PassengerDto?> Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Passenger.GetByIdAsync(request.Id);

        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Passenger.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Passenger",
            entityId: existing.Id.ToString(),
            details: $"Passager mis à jour: {existing.Name} {existing.LastName}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}