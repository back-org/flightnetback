using Flight.Application.CQRS.Commands.Airports;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using AirportEntity = Flight.Domain.Entities.Airport;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers CQRS de commande pour les aéroports.
/// </summary>
public class AirportCommandHandlerTests
{
    /// <summary>
    /// Crée un DTO valide pour les tests.
    /// </summary>
    private static AirportDto MakeDto(int id = 0)
    {
        return new AirportDto(
            id,
            "Ivato International Airport",
            1,
            State.Active,
            0
        );
    }

    /// <summary>
    /// Crée une entité valide pour les tests.
    /// </summary>
    private static AirportEntity MakeEntity(int id = 1)
    {
        return MakeDto(id).ToEntity();
    }

    /// <summary>
    /// Prépare les mocks du gestionnaire de repositories et du repository Airport.
    /// </summary>
    private static (Mock<IRepositoryManager> managerMock, Mock<IGenericRepository<AirportEntity>> repoMock) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<AirportEntity>>();
        var managerMock = new Mock<IRepositoryManager>();

        managerMock.Setup(m => m.Airport).Returns(repoMock.Object);

        return (managerMock, repoMock);
    }

    [Fact]
    public async Task CreateAirport_ValidCommand_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.AddAsync(It.IsAny<AirportEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateAirportCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new CreateAirportCommand(MakeDto(), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Ivato International Airport");

        repoMock.Verify(r => r.AddAsync(It.IsAny<AirportEntity>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAirport_ExistingAirport_ShouldUpdateAndReturnDto()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.Update(It.IsAny<AirportEntity>())).Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateAirportCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateAirportCommand(1, MakeDto(1), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        repoMock.Verify(r => r.Update(It.IsAny<AirportEntity>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAirport_NotFound_ShouldReturnNull()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((AirportEntity?)null);

        var handler = new UpdateAirportCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateAirportCommand(999, MakeDto(999), "unit-test"),
            CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAirport_ExistingAirport_ShouldReturnTrue()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new DeleteAirportCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteAirportCommand(1, "unit-test"),
            CancellationToken.None);

        result.Should().BeTrue();
        repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAirport_NotFound_ShouldReturnFalse()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((AirportEntity?)null);

        var handler = new DeleteAirportCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteAirportCommand(999, "unit-test"),
            CancellationToken.None);

        result.Should().BeFalse();
    }
}