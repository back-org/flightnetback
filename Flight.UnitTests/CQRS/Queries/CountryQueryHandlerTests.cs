using Flight.Application.CQRS.Queries.Countries;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using CountryEntity = Flight.Domain.Entities.Country;

namespace Flight.UnitTests.CQRS.Queries;

/// <summary>
/// Tests unitaires des handlers de requêtes CQRS pour les pays.
/// </summary>
public class CountryQueryHandlerTests
{
    private static CountryEntity MakeEntity(int id = 1)
        => new CountryDto(id, "Madagascar", "MG", "MDG").ToEntity();

    private static Mock<IRepositoryManager> SetupManager(Mock<IGenericRepository<CountryEntity>> repoMock)
    {
        var managerMock = new Mock<IRepositoryManager>();
        managerMock.Setup(m => m.Country).Returns(repoMock.Object);
        return managerMock;
    }

    [Fact]
    public async Task GetCountryById_Existing_ShouldReturnDto()
    {
        var repoMock = new Mock<IGenericRepository<CountryEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeEntity(1));
        var managerMock = SetupManager(repoMock);

        var handler = new GetCountryByIdQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetCountryByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Madagascar");
    }

    [Fact]
    public async Task GetCountryById_NotFound_ShouldReturnNull()
    {
        var repoMock = new Mock<IGenericRepository<CountryEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((CountryEntity?)null);
        var managerMock = SetupManager(repoMock);

        var handler = new GetCountryByIdQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetCountryByIdQuery(999), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllCountries_ShouldReturnDtos()
    {
        var repoMock = new Mock<IGenericRepository<CountryEntity>>();
        repoMock.Setup(r => r.AllAsync()).ReturnsAsync(new List<CountryEntity>
        {
            MakeEntity(1),
            new CountryDto(2, "France", "FR", "FRA").ToEntity()
        });

        var managerMock = SetupManager(repoMock);

        var handler = new GetAllCountriesQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAllCountriesQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }
}