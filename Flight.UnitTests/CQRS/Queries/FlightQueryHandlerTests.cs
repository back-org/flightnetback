using Flight.Application.CQRS.Queries.Flights;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using FlightEntity = Flight.Domain.Entities.Flight;

namespace Flight.UnitTests.CQRS.Queries;

/// <summary>
/// Tests unitaires des handlers de requêtes CQRS pour les vols.
/// </summary>
public class FlightQueryHandlerTests
{
    private static FlightEntity MakeEntity(int id = 1)
        => new FlightDto(id, "AF001", DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(5), 20, 150, 500f, 150f, 2, 1).ToEntity();

    private static Mock<IRepositoryManager> SetupManager(Mock<IGenericRepository<FlightEntity>> repoMock)
    {
        var managerMock = new Mock<IRepositoryManager>();
        managerMock.Setup(m => m.Flight).Returns(repoMock.Object);
        return managerMock;
    }

    [Fact]
    public async Task GetFlightById_Existing_ShouldReturnDto()
    {
        var repoMock = new Mock<IGenericRepository<FlightEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeEntity(1));
        var managerMock = SetupManager(repoMock);

        var handler = new GetFlightByIdQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetFlightByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Code.Should().Be("AF001");
    }

    [Fact]
    public async Task GetAllFlights_ShouldReturnDtos()
    {
        var repoMock = new Mock<IGenericRepository<FlightEntity>>();
        repoMock.Setup(r => r.AllAsync()).ReturnsAsync(new List<FlightEntity>
        {
            MakeEntity(1),
            new FlightDto(2, "AF002", DateTime.UtcNow.AddHours(3), DateTime.UtcNow.AddHours(6), 10, 120, 600f, 180f, 3, 1).ToEntity()
        });

        var managerMock = SetupManager(repoMock);

        var handler = new GetAllFlightsQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAllFlightsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }
}