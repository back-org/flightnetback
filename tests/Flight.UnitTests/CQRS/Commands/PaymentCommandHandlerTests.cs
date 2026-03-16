using Flight.Application.CQRS.Commands.Payments;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers de commandes pour les paiements.
/// </summary>
public class PaymentCommandHandlerTests
{
    private static Payment MakeEntity(int id = 1) => new()
    {
        Id = id,
        BookingId = 1,
        Amount = 150000,
        Currency = "MGA",
        PaymentMethod = "Cash",
        Status = "Paid",
        TransactionReference = "PAY001",
        PaidAt = DateTime.UtcNow
    };

    private static PaymentDto MakeDto(int id = 0) => new(
        id, 1, 150000, "MGA", "Cash", "Paid", "PAY001", DateTime.UtcNow);

    private static (
        Mock<IRepositoryManager> managerMock,
        Mock<IGenericRepository<Payment>> repoMock,
        Mock<IAuditTrailService> auditMock) Setup()
    {
        var repoMock = new Mock<IGenericRepository<Payment>>();
        var managerMock = new Mock<IRepositoryManager>();
        var auditMock = new Mock<IAuditTrailService>();

        managerMock.Setup(m => m.Payment).Returns(repoMock.Object);

        return (managerMock, repoMock, auditMock);
    }

    [Fact]
    public async Task CreatePayment_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Payment>())).ReturnsAsync(1);

        var handler = new CreatePaymentCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new CreatePaymentCommand(MakeDto(), "tester"), CancellationToken.None);

        result.Status.Should().Be("Paid");
    }

    [Fact]
    public async Task UpdatePayment_ShouldReturnNull_WhenNotFound()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Payment?)null);

        var handler = new UpdatePaymentCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new UpdatePaymentCommand(42, MakeDto(42), "tester"), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletePayment_ShouldReturnTrue_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var handler = new DeletePaymentCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeletePaymentCommand(1, "tester"), CancellationToken.None);

        result.Should().BeTrue();
    }
}