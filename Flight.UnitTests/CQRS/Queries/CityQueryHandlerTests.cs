using Flight.Application.CQRS.Queries.Cities;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using CityEntity = Flight.Domain.Entities.City;

namespace Flight.UnitTests.CQRS.Queries;

/// <summary>
/// Tests unitaires des handlers de requêtes CQRS pour les villes.
/// </summary>
public class CityQueryHandlerTests
{
    private static CityEntity MakeEntity(int id = 1)
        => new CityDto(id, "Antananarivo", 1, -18.8792, 47.5079).ToEntity();

    private static Mock<IRepositoryManager> SetupManager(Mock<IGenericRepository<CityEntity>> repoMock)
    {
        var managerMock = new Mock<IRepositoryManager>();
        managerMock.Setup(m => m.City).Returns(repoMock.Object);
        return managerMock;
    }

    [Fact]
    public async Task GetCityById_Existing_ShouldReturnDto()
    {
        var repoMock = new Mock<IGenericRepository<CityEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeEntity(1));
        var managerMock = SetupManager(repoMock);

        var handler = new GetCityByIdQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetCityByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Antananarivo");
    }

    [Fact]
    public async Task GetAllCities_ShouldReturnDtos()
    {
        var repoMock = new Mock<IGenericRepository<CityEntity>>();
        repoMock.Setup(r => r.AllAsync()).ReturnsAsync(new List<CityEntity>
        {
            MakeEntity(1),
            new CityDto(2, "Toamasina", 1, -18.1492, 49.4023).ToEntity()
        });

        var managerMock = SetupManager(repoMock);

        var handler = new GetAllCitiesQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAllCitiesQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }
}