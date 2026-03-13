using Flight.Application.CQRS.Queries.Bookings;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using BookingEntity = Flight.Domain.Entities.Booking;

namespace Flight.UnitTests.CQRS.Queries;

/// <summary>
/// Tests unitaires des handlers de requêtes CQRS pour les réservations.
/// </summary>
public class BookingQueryHandlerTests
{
    private static BookingEntity MakeEntity(int id = 1)
        => new BookingDto(id, Confort.Economy, 1, 1, Statut.Pending).ToEntity();

    private static Mock<IRepositoryManager> SetupManager(Mock<IGenericRepository<BookingEntity>> repoMock)
    {
        var managerMock = new Mock<IRepositoryManager>();
        managerMock.Setup(m => m.Booking).Returns(repoMock.Object);
        return managerMock;
    }

    [Fact]
    public async Task GetBookingById_Existing_ShouldReturnDto()
    {
        var repoMock = new Mock<IGenericRepository<BookingEntity>>();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MakeEntity(1));
        var managerMock = SetupManager(repoMock);

        var handler = new GetBookingByIdQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetBookingByIdQuery(1), CancellationToken.None);

        result.Should().NotBeNull();
        result!.PassengerId.Should().Be(1);
    }

    [Fact]
    public async Task GetAllBookings_ShouldReturnDtos()
    {
        var repoMock = new Mock<IGenericRepository<BookingEntity>>();
        repoMock.Setup(r => r.AllAsync()).ReturnsAsync(new List<BookingEntity>
        {
            MakeEntity(1),
            new BookingDto(2, Confort.Business, 2, 2, Statut.Confirmed).ToEntity()
        });

        var managerMock = SetupManager(repoMock);

        var handler = new GetAllBookingsQueryHandler(managerMock.Object);
        var result = await handler.Handle(new GetAllBookingsQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
    }
}