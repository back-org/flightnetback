using Flight.Application.CQRS.Commands.TaskItems;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers de commandes pour les tâches.
/// </summary>
public class TaskItemCommandHandlerTests
{
    private static TaskItem MakeEntity(int id = 1) => new()
    {
        Id = id,
        Title = "Vérifier vol",
        Description = "Contrôle avant départ",
        CreatedByUserId = 1,
        AssignedToUserId = 2,
        Priority = "High",
        Status = "Open",
        CreatedAt = DateTime.UtcNow
    };

    private static TaskItemDto MakeDto(int id = 0) => new(
        id, "Vérifier vol", "Contrôle avant départ", 1, 2, "High", "Open", null, DateTime.UtcNow);

    private static (
        Mock<IRepositoryManager> managerMock,
        Mock<IGenericRepository<TaskItem>> repoMock,
        Mock<IAuditTrailService> auditMock) Setup()
    {
        var repoMock = new Mock<IGenericRepository<TaskItem>>();
        var managerMock = new Mock<IRepositoryManager>();
        var auditMock = new Mock<IAuditTrailService>();

        managerMock.Setup(m => m.TaskItem).Returns(repoMock.Object);

        return (managerMock, repoMock, auditMock);
    }

    [Fact]
    public async Task CreateTaskItem_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>())).ReturnsAsync(1);

        var handler = new CreateTaskItemCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new CreateTaskItemCommand(MakeDto(), "tester"), CancellationToken.None);

        result.Title.Should().Be("Vérifier vol");
    }

    [Fact]
    public async Task DeleteTaskItem_ShouldReturnTrue_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var handler = new DeleteTaskItemCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeleteTaskItemCommand(1, "tester"), CancellationToken.None);

        result.Should().BeTrue();
    }
}