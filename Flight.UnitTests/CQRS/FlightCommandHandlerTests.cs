using Flight.Application.CQRS.Commands.Flights;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS;

/// <summary>
/// Tests unitaires pour les handlers CQRS de vol.
/// </summary>
public class FlightCommandHandlerTests
{
    private static Domain.Entities.Flight MakeFlight(int id = 1) =>
    new FlightDto(
        id, "AF001",
        DateTime.UtcNow.AddHours(2),
        DateTime.UtcNow.AddHours(5),
        20, 150,
        500f, 150f,
        2, 1
    ).ToEntity();

    private static FlightDto MakeDto(int id = 0) => new(
        id, "AF001",
        DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(5),
        20, 150, 500f, 150f, 2, 1);

    private static (Mock<IRepositoryManager>, Mock<IGenericRepository<Domain.Entities.Flight>>) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<Domain.Entities.Flight>>();
        var managerMock = new Mock<IRepositoryManager>();
        managerMock.Setup(m => m.Flight).Returns(repoMock.Object);
        return (managerMock, repoMock);
    }

    [Fact]
    public async Task CreateFlight_ValidCommand_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Flight>())).Returns((Task<int>)Task.CompletedTask);

        var handler = new CreateFlightCommandHandler(managerMock.Object, auditMock.Object);
        var result = await handler.Handle(new CreateFlightCommand(MakeDto()), CancellationToken.None);

        result.Should().NotBeNull();
        result.Code.Should().Be("AF001");
        repoMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Flight>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFlight_ExistingFlight_ShouldUpdateAndReturnDto()
    {
        var flight = MakeFlight(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(flight);
        repoMock.Setup(r => r.Update(It.IsAny<Domain.Entities.Flight>())).Returns((Task<int>)Task.CompletedTask);

        var handler = new UpdateFlightCommandHandler(managerMock.Object, auditMock.Object);
        var result = await handler.Handle(new UpdateFlightCommand(1, MakeDto(1)), CancellationToken.None);

        result.Should().NotBeNull();
        repoMock.Verify(r => r.Update(It.IsAny<Domain.Entities.Flight>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFlight_NotFound_ShouldReturnNull()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();
        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Domain.Entities.Flight?)null);

        var handler = new UpdateFlightCommandHandler(managerMock.Object, auditMock.Object);
        var result = await handler.Handle(new UpdateFlightCommand(999, MakeDto(999)), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteFlight_ExistingFlight_ShouldReturnTrue()
    {
        var flight = MakeFlight(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(flight);
        repoMock.Setup(r => r.Delete(It.IsAny<Domain.Entities.Flight>())).Returns((Task<int>)Task.CompletedTask);

        var handler = new DeleteFlightCommandHandler(managerMock.Object, auditMock.Object);
        var result = await handler.Handle(new DeleteFlightCommand(1), CancellationToken.None);

        result.Should().BeTrue();
        repoMock.Verify(r => r.Delete(It.IsAny<Domain.Entities.Flight>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFlight_NotFound_ShouldReturnFalse()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();
        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Domain.Entities.Flight?)null);

        var handler = new DeleteFlightCommandHandler(managerMock.Object, auditMock.Object);
        var result = await handler.Handle(new DeleteFlightCommand(999), CancellationToken.None);

        result.Should().BeFalse();
    }
}
