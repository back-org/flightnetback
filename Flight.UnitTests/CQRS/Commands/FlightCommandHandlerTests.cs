using Flight.Application.CQRS.Commands.Flights;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using FlightEntity = Flight.Domain.Entities.Flight;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires pour les handlers CQRS liés aux vols.
/// </summary>
public class FlightCommandHandlerTests
{
    /// <summary>
    /// Construit une entité métier <see cref="FlightEntity"/> valide pour les tests.
    /// </summary>
    /// <param name="id">Identifiant du vol à créer.</param>
    /// <returns>Une entité vol valide.</returns>
    private static FlightEntity MakeFlight(int id = 1)
    {
        return new FlightDto(
            id,
            "AF001",
            DateTime.UtcNow.AddHours(2),
            DateTime.UtcNow.AddHours(5),
            20,
            150,
            500f,
            150f,
            2,
            1
        ).ToEntity();
    }

    /// <summary>
    /// Construit un DTO <see cref="FlightDto"/> valide pour les tests.
    /// </summary>
    /// <param name="id">Identifiant du DTO à créer.</param>
    /// <returns>Un DTO de vol valide.</returns>
    private static FlightDto MakeDto(int id = 0)
    {
        return new FlightDto(
            id,
            "AF001",
            DateTime.UtcNow.AddHours(2),
            DateTime.UtcNow.AddHours(5),
            20,
            150,
            500f,
            150f,
            2,
            1
        );
    }

    /// <summary>
    /// Prépare les mocks du gestionnaire de repositories et du repository de vols.
    /// </summary>
    /// <returns>
    /// Un tuple contenant :
    /// - le mock du gestionnaire de repositories
    /// - le mock du repository de vols
    /// </returns>
    private static (Mock<IRepositoryManager> managerMock, Mock<IGenericRepository<FlightEntity>> repoMock) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<FlightEntity>>();
        var managerMock = new Mock<IRepositoryManager>();

        managerMock.Setup(m => m.Flight).Returns(repoMock.Object);

        return (managerMock, repoMock);
    }

    /// <summary>
    /// Vérifie qu'une commande de création valide crée bien un vol
    /// et retourne le DTO correspondant.
    /// </summary>
    [Fact]
    public async Task CreateFlight_ValidCommand_ShouldCreateAndReturnDto()
    {
        // Arrange
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock
            .Setup(r => r.AddAsync(It.IsAny<FlightEntity>()))
            .Returns((Task<int>)Task.CompletedTask);

        auditMock
            .Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateFlightCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new CreateFlightCommand(MakeDto(), "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be("AF001");

        repoMock.Verify(r => r.AddAsync(It.IsAny<FlightEntity>()), Times.Once);
        auditMock.Verify(a => a.RecordAsync(
            "CREATE",
            "Flight",
            It.IsAny<string>(),
            It.IsAny<string>(),
            "unit-test",
            It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Vérifie qu'une mise à jour sur un vol existant fonctionne correctement.
    /// </summary>
    [Fact]
    public async Task UpdateFlight_ExistingFlight_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var flight = MakeFlight(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(flight);

        repoMock
            .Setup(r => r.Update(It.IsAny<FlightEntity>()))
            .Returns((Task<int>)Task.CompletedTask);

        auditMock
            .Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateFlightCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new UpdateFlightCommand(1, MakeDto(1), "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);

        repoMock.Verify(r => r.Update(It.IsAny<FlightEntity>()), Times.Once);
        auditMock.Verify(a => a.RecordAsync(
            "UPDATE",
            "Flight",
            It.IsAny<string>(),
            It.IsAny<string>(),
            "unit-test",
            It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Vérifie qu'une mise à jour sur un vol inexistant retourne null.
    /// </summary>
    [Fact]
    public async Task UpdateFlight_NotFound_ShouldReturnNull()
    {
        // Arrange
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((FlightEntity?)null);

        var handler = new UpdateFlightCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new UpdateFlightCommand(999, MakeDto(999), "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().BeNull();
        repoMock.Verify(r => r.Update(It.IsAny<FlightEntity>()), Times.Never);
    }

    /// <summary>
    /// Vérifie qu'une suppression sur un vol existant retourne true
    /// et appelle bien le repository.
    /// </summary>
    [Fact]
    public async Task DeleteFlight_ExistingFlight_ShouldReturnTrue()
    {
        // Arrange
        var flight = MakeFlight(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(flight);

        repoMock
            .Setup(r => r.DeleteAsync(1))
            .Returns((Task<int>)Task.CompletedTask);

        auditMock
            .Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new DeleteFlightCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new DeleteFlightCommand(1, "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        auditMock.Verify(a => a.RecordAsync(
            "DELETE",
            "Flight",
            "1",
            It.IsAny<string>(),
            "unit-test",
            It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Vérifie qu'une suppression sur un vol inexistant retourne false.
    /// </summary>
    [Fact]
    public async Task DeleteFlight_NotFound_ShouldReturnFalse()
    {
        // Arrange
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock
            .Setup(r => r.GetByIdAsync(999))
            .ReturnsAsync((FlightEntity?)null);

        var handler = new DeleteFlightCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new DeleteFlightCommand(999, "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        repoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}