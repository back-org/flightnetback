using Flight.Application.CQRS.Commands.Airlines;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using AirlineEntity = Flight.Domain.Entities.Airline;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers CQRS de commande pour les compagnies aériennes.
/// </summary>
public class AirlineCommandHandlerTests
{
    /// <summary>
    /// Crée un DTO valide pour les tests.
    /// </summary>
    private static AirlineDto MakeDto(int id = 0)
    {
        return new AirlineDto(
            id,
            "Air France",
            State.Active,
            0
        );
    }

    /// <summary>
    /// Crée une entité valide pour les tests.
    /// </summary>
    private static AirlineEntity MakeEntity(int id = 1)
    {
        return MakeDto(id).ToEntity();
    }

    /// <summary>
    /// Prépare les mocks du gestionnaire de repositories et du repository Airline.
    /// </summary>
    private static (Mock<IRepositoryManager> managerMock, Mock<IGenericRepository<AirlineEntity>> repoMock) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<AirlineEntity>>();
        var managerMock = new Mock<IRepositoryManager>();

        managerMock.Setup(m => m.Airline).Returns(repoMock.Object);

        return (managerMock, repoMock);
    }

    /// <summary>
    /// Vérifie qu'une création de compagnie aérienne fonctionne correctement.
    /// </summary>
    [Fact]
    public async Task CreateAirline_ValidCommand_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.AddAsync(It.IsAny<AirlineEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateAirlineCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new CreateAirlineCommand(MakeDto(), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Air France");

        repoMock.Verify(r => r.AddAsync(It.IsAny<AirlineEntity>()), Times.Once);
    }

    /// <summary>
    /// Vérifie qu'une mise à jour d'une compagnie existante fonctionne correctement.
    /// </summary>
    [Fact]
    public async Task UpdateAirline_ExistingAirline_ShouldUpdateAndReturnDto()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.Update(It.IsAny<AirlineEntity>())).Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateAirlineCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateAirlineCommand(1, MakeDto(1), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);

        repoMock.Verify(r => r.Update(It.IsAny<AirlineEntity>()), Times.Once);
    }

    /// <summary>
    /// Vérifie qu'une mise à jour sur une compagnie inexistante retourne null.
    /// </summary>
    [Fact]
    public async Task UpdateAirline_NotFound_ShouldReturnNull()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((AirlineEntity?)null);

        var handler = new UpdateAirlineCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateAirlineCommand(999, MakeDto(999), "unit-test"),
            CancellationToken.None);

        result.Should().BeNull();
        repoMock.Verify(r => r.Update(It.IsAny<AirlineEntity>()), Times.Never);
    }

    /// <summary>
    /// Vérifie qu'une suppression d'une compagnie existante retourne true.
    /// </summary>
    [Fact]
    public async Task DeleteAirline_ExistingAirline_ShouldReturnTrue()
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

        var handler = new DeleteAirlineCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteAirlineCommand(1, "unit-test"),
            CancellationToken.None);

        result.Should().BeTrue();
        repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    /// <summary>
    /// Vérifie qu'une suppression sur une compagnie inexistante retourne false.
    /// </summary>
    [Fact]
    public async Task DeleteAirline_NotFound_ShouldReturnFalse()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((AirlineEntity?)null);

        var handler = new DeleteAirlineCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteAirlineCommand(999, "unit-test"),
            CancellationToken.None);

        result.Should().BeFalse();
        repoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}