using Flight.Application.CQRS.Commands.Vehicles;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using VehicleEntity = Flight.Domain.Entities.Vehicle;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers CQRS pour les véhicules.
/// </summary>
public class VehicleCommandHandlerTests
{
    private static VehicleDto MakeDto(int id = 0)
    {
        return new VehicleDto(
            id,
            "1234 TAA",
            "Toyota",
            "Land Cruiser",
            2022,
            120.5f
        );
    }

    private static VehicleEntity MakeEntity(int id = 1)
    {
        return MakeDto(id).ToEntity();
    }

    private static (Mock<IRepositoryManager>, Mock<IGenericRepository<VehicleEntity>>) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<VehicleEntity>>();
        var managerMock = new Mock<IRepositoryManager>();

        managerMock.Setup(m => m.Vehicle).Returns(repoMock.Object);

        return (managerMock, repoMock);
    }

    [Fact]
    public async Task CreateVehicle_ShouldCreateVehicle()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.AddAsync(It.IsAny<VehicleEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        var handler = new CreateVehicleCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new CreateVehicleCommand(MakeDto(), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        result.LicensePlate.Should().Be("1234 TAA");
    }

    [Fact]
    public async Task UpdateVehicle_ShouldUpdateVehicle()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(entity);

        repoMock.Setup(r => r.Update(It.IsAny<VehicleEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        var handler = new UpdateVehicleCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateVehicleCommand(1, MakeDto(1), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteVehicle_ShouldDeleteVehicle()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(entity);

        repoMock.Setup(r => r.DeleteAsync(1))
                .Returns((Task<int>)Task.CompletedTask);

        var handler = new DeleteVehicleCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteVehicleCommand(1, "unit-test"),
            CancellationToken.None);

        result.Should().BeTrue();
    }
}