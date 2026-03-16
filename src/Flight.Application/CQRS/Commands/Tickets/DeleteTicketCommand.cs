/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Commands/Tickets' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Tickets;

/// <summary>
/// Commande permettant de supprimer un billet.
/// </summary>
public record DeleteTicketCommand(int Id, string PerformedBy) : IRequest<bool>;

/// <summary>
/// Handler chargé de supprimer un billet.
/// </summary>
public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, bool>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public DeleteTicketCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Ticket.GetByIdAsync(request.Id);
        if (existing is null)
        {
            return false;
        }

        await _manager.Ticket.DeleteAsync(request.Id);

        await _audit.RecordAsync(
            action: "DELETE",
            entityName: "Ticket",
            entityId: request.Id.ToString(),
            details: $"Billet supprimé : {existing.TicketNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return true;
    }
}