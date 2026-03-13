using Flight.Application.CQRS.Queries.Airlines;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using AirlineEntity = Flight.Domain.Entities.Airline;

namespace Flight.UnitTests.CQRS.Queries;

/// <summary>
/// Tests unitaires des handlers de requêtes CQRS pour les compagnies aériennes.
/// </summary>
public class AirlineQueryHandlerTests
{
    private static AirlineEntity MakeEntity(int id = 1)
        => new AirlineDto(id, "Air France", State.Active, 0).ToEntity();

    private static Mock<IRepositoryManager> SetupManager(Mock<IGenericRepository<AirlineEntity>> repoMock)
    {
        var managerMock = new Mock<IRepositoryManager>();
        managerMock.Setup(m => m.Airline).Returns(repoMock.Object);
        return managerMock;
    }

    [Fact]
    public async Task GetAirlineById_Existing_ShouldReturnDto()
    {
        var repoMock = new Mock<IGenericRepository<AirlineEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeEntity(1));
        var managerMock = SetupManager(repoMock);

        var handler = new GetAirlineByIdQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAirlineByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Air France");
    }

    [Fact]
    public async Task GetAllAirlines_ShouldReturnDtos()
    {
        var repoMock = new Mock<IGenericRepository<AirlineEntity>>();
        repoMock.Setup(r => r.AllAsync()).ReturnsAsync(new List<AirlineEntity>
        {
            MakeEntity(1),
            new AirlineDto(2, "Air Madagascar", State.Active, 0).ToEntity()
        });

        var managerMock = SetupManager(repoMock);

        var handler = new GetAllAirlinesQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAllAirlinesQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }
}