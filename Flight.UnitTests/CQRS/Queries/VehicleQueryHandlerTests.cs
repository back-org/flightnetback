using Flight.Application.CQRS.Queries.Vehicles;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using VehicleEntity = Flight.Domain.Entities.Vehicle;

namespace Flight.UnitTests.CQRS.Queries;

/// <summary>
/// Tests unitaires des handlers de requêtes CQRS pour les véhicules.
/// </summary>
public class VehicleQueryHandlerTests
{
    private static VehicleEntity MakeEntity(int id = 1)
        => new VehicleDto(id, "1234 TAA", "Toyota", "Land Cruiser", 2022, 120.5f).ToEntity();

    private static Mock<IRepositoryManager> SetupManager(Mock<IGenericRepository<VehicleEntity>> repoMock)
    {
        var managerMock = new Mock<IRepositoryManager>();
        managerMock.Setup(m => m.Vehicle).Returns(repoMock.Object);
        return managerMock;
    }

    [Fact]
    public async Task GetVehicleById_Existing_ShouldReturnDto()
    {
        var repoMock = new Mock<IGenericRepository<VehicleEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeEntity(1));
        var managerMock = SetupManager(repoMock);

        var handler = new GetVehicleByIdQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetVehicleByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result!.LicensePlate.Should().Be("1234 TAA");
    }

    [Fact]
    public async Task GetAllVehicles_ShouldReturnDtos()
    {
        var repoMock = new Mock<IGenericRepository<VehicleEntity>>();
        repoMock.Setup(r => r.AllAsync()).ReturnsAsync(new List<VehicleEntity>
        {
            MakeEntity(1),
            new VehicleDto(2, "5678 TAB", "Nissan", "Patrol", 2023, 150f).ToEntity()
        });

        var managerMock = SetupManager(repoMock);

        var handler = new GetAllVehiclesQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAllVehiclesQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }
}