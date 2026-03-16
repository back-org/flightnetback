using Flight.Application.CQRS.Commands.CrewMembers;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers de commandes pour les membres d'équipe.
/// </summary>
public class CrewMemberCommandHandlerTests
{
    private static CrewMember MakeEntity(int id = 1) => new()
    {
        Id = id,
        UserId = 1,
        EmployeeNumber = "EMP001",
        Position = "Pilot",
        LicenseNumber = "LIC001",
        HireDate = DateTime.UtcNow.AddYears(-2),
        Status = "Active"
    };

    private static CrewMemberDto MakeDto(int id = 0) => new(
        id, 1, "EMP001", "Pilot", "LIC001", DateTime.UtcNow.AddYears(-2), "Active");

    private static (
        Mock<IRepositoryManager> managerMock,
        Mock<IGenericRepository<CrewMember>> repoMock,
        Mock<IAuditTrailService> auditMock) Setup()
    {
        var repoMock = new Mock<IGenericRepository<CrewMember>>();
        var managerMock = new Mock<IRepositoryManager>();
        var auditMock = new Mock<IAuditTrailService>();

        managerMock.Setup(m => m.CrewMember).Returns(repoMock.Object);

        return (managerMock, repoMock, auditMock);
    }

    [Fact]
    public async Task CreateCrewMember_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.AddAsync(It.IsAny<CrewMember>())).ReturnsAsync(1);

        var handler = new CreateCrewMemberCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new CreateCrewMemberCommand(MakeDto(), "tester"), CancellationToken.None);

        result.EmployeeNumber.Should().Be("EMP001");
    }

    [Fact]
    public async Task UpdateCrewMember_ShouldReturnNull_WhenNotFound()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((CrewMember?)null);

        var handler = new UpdateCrewMemberCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new UpdateCrewMemberCommand(42, MakeDto(42), "tester"), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCrewMember_ShouldReturnTrue_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var handler = new DeleteCrewMemberCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeleteCrewMemberCommand(1, "tester"), CancellationToken.None);

        result.Should().BeTrue();
    }
}