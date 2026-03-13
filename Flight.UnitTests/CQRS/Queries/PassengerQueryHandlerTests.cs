using Flight.Application.CQRS.Queries.Passengers;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using PassengerEntity = Flight.Domain.Entities.Passenger;

namespace Flight.UnitTests.CQRS.Queries;

/// <summary>
/// Tests unitaires des handlers de requêtes CQRS pour les passagers.
/// </summary>
public class PassengerQueryHandlerTests
{
    private static PassengerEntity MakeEntity(int id = 1)
        => new PassengerDto(id, "Jean", "", "Dupont", "jean@test.com", "+261340000000", "Tana", Genre.Male).ToEntity();

    private static Mock<IRepositoryManager> SetupManager(Mock<IGenericRepository<PassengerEntity>> repoMock)
    {
        var managerMock = new Mock<IRepositoryManager>();
        managerMock.Setup(m => m.Passenger).Returns(repoMock.Object);
        return managerMock;
    }

    [Fact]
    public async Task GetPassengerById_Existing_ShouldReturnDto()
    {
        var repoMock = new Mock<IGenericRepository<PassengerEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeEntity(1));
        var managerMock = SetupManager(repoMock);

        var handler = new GetPassengerByIdQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetPassengerByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Name.Should().Be("Jean");
    }

    [Fact]
    public async Task GetAllPassengers_ShouldReturnDtos()
    {
        var repoMock = new Mock<IGenericRepository<PassengerEntity>>();
        repoMock.Setup(r => r.AllAsync()).ReturnsAsync(new List<PassengerEntity>
        {
            MakeEntity(1),
            new PassengerDto(2, "Sarah", "", "Rabe", "sarah@test.com", "+261320000000", "Fianarantsoa", Genre.Female).ToEntity()
        });

        var managerMock = SetupManager(repoMock);

        var handler = new GetAllPassengersQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAllPassengersQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }
}