using Flight.Application.CQRS.Commands.Tickets;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers de commandes pour les billets.
/// </summary>
public class TicketCommandHandlerTests
{
    private static Ticket MakeEntity(int id = 1) => new()
    {
        Id = id,
        TicketNumber = "TCK001",
        BookingId = 1,
        PassengerId = 1,
        IssuedAt = DateTime.UtcNow,
        Status = "Issued"
    };

    private static TicketDto MakeDto(int id = 0) => new(
        id, "TCK001", 1, 1, DateTime.UtcNow, "Issued");

    private static (
        Mock<IRepositoryManager> managerMock,
        Mock<IGenericRepository<Ticket>> repoMock,
        Mock<IAuditTrailService> auditMock) Setup()
    {
        var repoMock = new Mock<IGenericRepository<Ticket>>();
        var managerMock = new Mock<IRepositoryManager>();
        var auditMock = new Mock<IAuditTrailService>();

        managerMock.Setup(m => m.Ticket).Returns(repoMock.Object);

        return (managerMock, repoMock, auditMock);
    }

    [Fact]
    public async Task CreateTicket_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Ticket>())).ReturnsAsync(1);

        var handler = new CreateTicketCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new CreateTicketCommand(MakeDto(), "tester"), CancellationToken.None);

        result.TicketNumber.Should().Be("TCK001");
    }

    [Fact]
    public async Task DeleteTicket_ShouldReturnTrue_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var handler = new DeleteTicketCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeleteTicketCommand(1, "tester"), CancellationToken.None);

        result.Should().BeTrue();
    }
}