using Flight.Application.CQRS.Commands.Bookings;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using BookingEntity = Flight.Domain.Entities.Booking;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers CQRS de commande pour les réservations.
/// </summary>
public class BookingCommandHandlerTests
{
    /// <summary>
    /// Crée un DTO valide pour les tests.
    /// </summary>
    private static BookingDto MakeDto(int id = 0)
    {
        return new BookingDto(
            id,
            Confort.Economy,
            1,
            1,
            Statut.Pending
        );
    }

    /// <summary>
    /// Crée une entité valide pour les tests.
    /// </summary>
    private static BookingEntity MakeEntity(int id = 1)
    {
        return MakeDto(id).ToEntity();
    }

    /// <summary>
    /// Prépare les mocks du gestionnaire de repositories et du repository Booking.
    /// </summary>
    private static (Mock<IRepositoryManager> managerMock, Mock<IGenericRepository<BookingEntity>> repoMock) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<BookingEntity>>();
        var managerMock = new Mock<IRepositoryManager>();

        managerMock.Setup(m => m.Booking).Returns(repoMock.Object);

        return (managerMock, repoMock);
    }

    [Fact]
    public async Task CreateBooking_ValidCommand_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.AddAsync(It.IsAny<BookingEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateBookingCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new CreateBookingCommand(MakeDto(), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        result.FlightId.Should().Be(1);

        repoMock.Verify(r => r.AddAsync(It.IsAny<BookingEntity>()), Times.Once);
    }

    [Fact]
    public async Task UpdateBooking_ExistingBooking_ShouldUpdateAndReturnDto()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.Update(It.IsAny<BookingEntity>())).Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateBookingCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateBookingCommand(1, MakeDto(1), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        repoMock.Verify(r => r.Update(It.IsAny<BookingEntity>()), Times.Once);
    }

    [Fact]
    public async Task UpdateBooking_NotFound_ShouldReturnNull()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((BookingEntity?)null);

        var handler = new UpdateBookingCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateBookingCommand(999, MakeDto(999), "unit-test"),
            CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteBooking_ExistingBooking_ShouldReturnTrue()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new DeleteBookingCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteBookingCommand(1, "unit-test"),
            CancellationToken.None);

        result.Should().BeTrue();
        repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteBooking_NotFound_ShouldReturnFalse()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((BookingEntity?)null);

        var handler = new DeleteBookingCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteBookingCommand(999, "unit-test"),
            CancellationToken.None);

        result.Should().BeFalse();
    }
}