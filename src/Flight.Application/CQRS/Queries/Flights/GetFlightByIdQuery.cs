/*
 * Rôle métier du fichier: Implémenter les commandes et requêtes métier via le pattern CQRS.
 * Description: Ce fichier participe au sous-domaine 'Flight.Application/CQRS/Queries/Flights' et contribue au fonctionnement professionnel de la plateforme de gestion de vols.
 */

using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Queries.Flights;

/// <summary>
/// Requête MediatR permettant de récupérer un vol à partir de son identifiant.
/// </summary>
public record GetFlightByIdQuery(int Id) : IRequest<FlightDto?>;

/// <summary>
/// Handler chargé de traiter la récupération d'un vol par son identifiant.
/// </summary>
public class GetFlightByIdQueryHandler : IRequestHandler<GetFlightByIdQuery, FlightDto?>
{
    private readonly IRepositoryManager _manager;

    /// <summary>
    /// Initialise une nouvelle instance du handler de récupération d'un vol.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories.</param>
    public GetFlightByIdQueryHandler(IRepositoryManager manager)
    {
        _manager = manager;
    }

    /// <summary>
    /// Exécute la requête de récupération d'un vol par identifiant.
    /// </summary>
    /// <param name="request">Requête contenant l'identifiant du vol.</param>
    /// <param name="cancellationToken">Jeton d'annulation.</param>
    /// <returns>Le DTO du vol si trouvé, sinon <c>null</c>.</returns>
    public async Task<FlightDto?> Handle(GetFlightByIdQuery request, CancellationToken cancellationToken)
    {
        var flight = await _manager.Flight.GetByIdAsync(request.Id);
        return flight?.ToDto();
    }
}