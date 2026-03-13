using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.Flights;

/// <summary>
/// Commande MediatR pour la mise à jour d'un vol.
/// </summary>
/// <param name="Id">Identifiant du vol à mettre à jour.</param>
/// <param name="Dto">Données mises à jour du vol.</param>
public record UpdateFlightCommand(int Id, FlightDto Dto) : IRequest<FlightDto?>;

/// <summary>
/// Handler chargé de traiter la mise à jour d'un vol.
/// </summary>
public class UpdateFlightCommandHandler : IRequestHandler<UpdateFlightCommand, FlightDto?>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    /// <summary>
    /// Initialise une nouvelle instance du handler de mise à jour d'un vol.
    /// </summary>
    /// <param name="manager">Gestionnaire central des repositories.</param>
    /// <param name="audit">Service d'audit pour tracer les opérations métier.</param>
    public UpdateFlightCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    /// <summary>
    /// Exécute la commande de mise à jour d'un vol.
    /// </summary>
    /// <param name="request">Commande contenant l'identifiant et les nouvelles données du vol.</param>
    /// <param name="cancellationToken">Jeton d'annulation.</param>
    /// <returns>Le DTO du vol mis à jour, ou <c>null</c> si le vol n'existe pas.</returns>
    public async Task<FlightDto?> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
    {
        var existing = await _manager.Flight.GetByIdAsync(request.Id);

        if (existing is null)
        {
            return null;
        }

        existing.UpdateEntity(request.Dto);
        await _manager.Flight.Update(existing);

        await _audit.RecordAsync(
            action: "UPDATE",
            entityName: "Flight",
            entityId: existing.Id.ToString(),
            details: $"Vol mis à jour: {existing.Code}",
            cancellationToken: cancellationToken);

        return existing.ToDto();
    }
}