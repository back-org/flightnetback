using Flight.Application.CQRS.Queries.Airports;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using AirportEntity = Flight.Domain.Entities.Airport;

namespace Flight.UnitTests.CQRS.Queries;

/// <summary>
/// Tests unitaires des handlers de requêtes CQRS pour les aéroports.
/// </summary>
public class AirportQueryHandlerTests
{
    private static AirportEntity MakeEntity(int id = 1)
        => new AirportDto(id, "Ivato", 1, State.Active, 0).ToEntity();

    private static Mock<IRepositoryManager> SetupManager(Mock<IGenericRepository<AirportEntity>> repoMock)
    {
        var managerMock = new Mock<IRepositoryManager>();
        managerMock.Setup(m => m.Airport).Returns(repoMock.Object);
        return managerMock;
    }

    [Fact]
    public async Task GetAirportById_Existing_ShouldReturnDto()
    {
        var repoMock = new Mock<IGenericRepository<AirportEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeEntity(1));
        var managerMock = SetupManager(repoMock);

        var handler = new GetAirportByIdQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAirportByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Ivato");
    }

    [Fact]
    public async Task GetAllAirports_ShouldReturnDtos()
    {
        var repoMock = new Mock<IGenericRepository<AirportEntity>>();
        repoMock.Setup(r => r.AllAsync()).ReturnsAsync(new List<AirportEntity>
        {
            MakeEntity(1),
            new AirportDto(2, "Fascene", 2, State.Active, 0).ToEntity()
        });

        var managerMock = SetupManager(repoMock);

        var handler = new GetAllAirportsQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAllAirportsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }
}