using Flight.Application.CQRS.Commands.Aircrafts;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers de commandes pour les avions.
/// </summary>
public class AircraftCommandHandlerTests
{
    private static Aircraft MakeEntity(int id = 1) => new()
    {
        Id = id,
        RegistrationNumber = "5R-ABC",
        Manufacturer = "Airbus",
        Model = "A320",
        BusinessCapacity = 12,
        EconomyCapacity = 120,
        Status = "Available"
    };

    private static AircraftDto MakeDto(int id = 0) => new(
        id, "5R-ABC", "Airbus", "A320", 12, 120, "Available", null);

    private static (
        Mock<IRepositoryManager> managerMock,
        Mock<IGenericRepository<Aircraft>> repoMock,
        Mock<IAuditTrailService> auditMock) Setup()
    {
        var repoMock = new Mock<IGenericRepository<Aircraft>>();
        var managerMock = new Mock<IRepositoryManager>();
        var auditMock = new Mock<IAuditTrailService>();

        managerMock.Setup(m => m.Aircraft).Returns(repoMock.Object);

        return (managerMock, repoMock, auditMock);
    }

    [Fact]
    public async Task CreateAircraft_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Aircraft>())).ReturnsAsync(1);

        var handler = new CreateAircraftCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new CreateAircraftCommand(MakeDto(), "tester"), CancellationToken.None);

        result.RegistrationNumber.Should().Be("5R-ABC");
    }

    [Fact]
    public async Task UpdateAircraft_ShouldReturnNull_WhenNotFound()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Aircraft?)null);

        var handler = new UpdateAircraftCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new UpdateAircraftCommand(99, MakeDto(99), "tester"), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAircraft_ShouldReturnTrue_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var handler = new DeleteAircraftCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeleteAircraftCommand(1, "tester"), CancellationToken.None);

        result.Should().BeTrue();
    }
}