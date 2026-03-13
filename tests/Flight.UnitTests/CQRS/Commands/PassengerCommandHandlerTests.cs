using Flight.Application.CQRS.Commands.Passengers;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using PassengerEntity = Flight.Domain.Entities.Passenger;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers CQRS pour les passagers.
/// </summary>
public class PassengerCommandHandlerTests
{
    private static PassengerDto MakeDto(int id = 0)
    {
        return new PassengerDto(
            id,
            "Jean",
            "",
            "Dupont",
            "jean.dupont@test.com",
            "+261341234567",
            "Antananarivo",
            Genre.Male
        );
    }

    private static PassengerEntity MakeEntity(int id = 1)
    {
        return MakeDto(id).ToEntity();
    }

    private static (Mock<IRepositoryManager>, Mock<IGenericRepository<PassengerEntity>>) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<PassengerEntity>>();
        var managerMock = new Mock<IRepositoryManager>();

        managerMock.Setup(m => m.Passenger).Returns(repoMock.Object);

        return (managerMock, repoMock);
    }

    [Fact]
    public async Task CreatePassenger_ShouldCreatePassenger()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.AddAsync(It.IsAny<PassengerEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        var handler = new CreatePassengerCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new CreatePassengerCommand(MakeDto(), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Jean");
    }

    [Fact]
    public async Task UpdatePassenger_ShouldUpdatePassenger()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(entity);

        repoMock.Setup(r => r.Update(It.IsAny<PassengerEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        var handler = new UpdatePassengerCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdatePassengerCommand(1, MakeDto(1), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task DeletePassenger_ShouldDeletePassenger()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(entity);

        repoMock.Setup(r => r.DeleteAsync(1))
                .Returns((Task<int>)Task.CompletedTask);

        var handler = new DeletePassengerCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeletePassengerCommand(1, "unit-test"),
            CancellationToken.None);

        result.Should().BeTrue();
    }
}