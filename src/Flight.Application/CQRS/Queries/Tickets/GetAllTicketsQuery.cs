/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Tickets' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Tickets;

/// <summary>
/// Requête permettant de récupérer tous les billets.
/// </summary>
public record GetAllTicketsQuery() : IRequest<IEnumerable<TicketDto>>;

/// <summary>
/// Handler chargé de récupérer tous les billets.
/// </summary>
public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, IEnumerable<TicketDto>>
{
    private readonly IRepositoryManager _manager;

    public GetAllTicketsQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    public async Task<IEnumerable<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
    {
        var entities = await _manager.Ticket.AllAsync();
        return entities.Select(x => x.ToDto());
    }
}