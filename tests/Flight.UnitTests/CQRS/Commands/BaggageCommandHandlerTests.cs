using Flight.Application.CQRS.Commands.Baggages;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers de commandes pour les bagages.
/// </summary>
public class BaggageCommandHandlerTests
{
    private static Baggage MakeEntity(int id = 1) => new()
    {
        Id = id,
        BookingId = 1,
        PassengerId = 1,
        FlightId = 1,
        TagNumber = "BG001",
        Weight = 23,
        Type = "Checked",
        Status = "CheckedIn"
    };

    private static BaggageDto MakeDto(int id = 0) => new(
        id, 1, 1, 1, "BG001", 23, "Checked", "CheckedIn");

    private static (
        Mock<IRepositoryManager> managerMock,
        Mock<IGenericRepository<Baggage>> repoMock,
        Mock<IAuditTrailService> auditMock) Setup()
    {
        var repoMock = new Mock<IGenericRepository<Baggage>>();
        var managerMock = new Mock<IRepositoryManager>();
        var auditMock = new Mock<IAuditTrailService>();

        managerMock.Setup(m => m.Baggage).Returns(repoMock.Object);

        return (managerMock, repoMock, auditMock);
    }

    [Fact]
    public async Task CreateBaggage_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Baggage>())).ReturnsAsync(1);

        var handler = new CreateBaggageCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new CreateBaggageCommand(MakeDto(), "tester"), CancellationToken.None);

        result.TagNumber.Should().Be("BG001");
    }

    [Fact]
    public async Task DeleteBaggage_ShouldReturnTrue_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var handler = new DeleteBaggageCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeleteBaggageCommand(1, "tester"), CancellationToken.None);

        result.Should().BeTrue();
    }
}