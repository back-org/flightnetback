using Flight.Application.DTOs;
using Flight.Infrastructure.Interfaces;
using MediatR;

namespace Flight.Application.CQRS.Commands.CrewMembers;

/// <summary>
/// Commande permettant de créer un membre d'équipe.
/// </summary>
public record CreateCrewMemberCommand(CrewMemberDto Dto, string PerformedBy) : IRequest<CrewMemberDto>;

/// <summary>
/// Handler chargé de créer un membre d'équipe.
/// </summary>
public class CreateCrewMemberCommandHandler : IRequestHandler<CreateCrewMemberCommand, CrewMemberDto>
{
    private readonly IRepositoryManager _manager;
    private readonly IAuditTrailService _audit;

    public CreateCrewMemberCommandHandler(IRepositoryManager manager, IAuditTrailService audit)
    {
        _manager = manager;
        _audit = audit;
    }

    public async Task<CrewMemberDto> Handle(CreateCrewMemberCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.ToEntity();

        await _manager.CrewMember.AddAsync(entity);

        await _audit.RecordAsync(
            action: "CREATE",
            entityName: "CrewMember",
            entityId: entity.Id.ToString(),
            details: $"Membre d'équipe créé : {entity.EmployeeNumber}",
            performedBy: request.PerformedBy,
            cancellationToken: cancellationToken);

        return entity.ToDto();
    }
}