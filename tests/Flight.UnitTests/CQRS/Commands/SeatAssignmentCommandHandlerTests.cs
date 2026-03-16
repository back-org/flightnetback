using Flight.Application.CQRS.Commands.SeatAssignments;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers de commandes pour les attributions de sièges.
/// </summary>
public class SeatAssignmentCommandHandlerTests
{
    private static SeatAssignment MakeEntity(int id = 1) => new()
    {
        Id = id,
        FlightId = 1,
        PassengerId = 1,
        SeatNumber = "12A",
        SeatClass = "Economy"
    };

    private static SeatAssignmentDto MakeDto(int id = 0) => new(id, 1, 1, "12A", "Economy");

    private static (
        Mock<IRepositoryManager> managerMock,
        Mock<IGenericRepository<SeatAssignment>> repoMock,
        Mock<IAuditTrailService> auditMock) Setup()
    {
        var repoMock = new Mock<IGenericRepository<SeatAssignment>>();
        var managerMock = new Mock<IRepositoryManager>();
        var auditMock = new Mock<IAuditTrailService>();

        managerMock.Setup(m => m.SeatAssignment).Returns(repoMock.Object);

        return (managerMock, repoMock, auditMock);
    }

    [Fact]
    public async Task CreateSeatAssignment_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.AddAsync(It.IsAny<SeatAssignment>())).ReturnsAsync(1);

        var handler = new CreateSeatAssignmentCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new CreateSeatAssignmentCommand(MakeDto(), "tester"), CancellationToken.None);

        result.SeatNumber.Should().Be("12A");
    }

    [Fact]
    public async Task DeleteSeatAssignment_ShouldReturnTrue_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var handler = new DeleteSeatAssignmentCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeleteSeatAssignmentCommand(1, "tester"), CancellationToken.None);

        result.Should().BeTrue();
    }
}