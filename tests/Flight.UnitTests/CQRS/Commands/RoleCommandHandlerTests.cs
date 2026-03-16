using Flight.Application.CQRS.Commands.Roles;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers de commandes pour les rôles.
/// </summary>
public class RoleCommandHandlerTests
{
    private static Role MakeEntity(int id = 1) => new()
    {
        Id = id,
        Name = "Admin",
        Description = "Administrateur"
    };

    private static RoleDto MakeDto(int id = 0) => new(id, "Admin", "Administrateur");

    private static (
        Mock<IRepositoryManager> managerMock,
        Mock<IGenericRepository<Role>> repoMock,
        Mock<IAuditTrailService> auditMock) Setup()
    {
        var repoMock = new Mock<IGenericRepository<Role>>();
        var managerMock = new Mock<IRepositoryManager>();
        var auditMock = new Mock<IAuditTrailService>();

        managerMock.Setup(m => m.Role).Returns(repoMock.Object);

        return (managerMock, repoMock, auditMock);
    }

    [Fact]
    public async Task CreateRole_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Role>())).ReturnsAsync(1);

        var handler = new CreateRoleCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new CreateRoleCommand(MakeDto(), "tester"), CancellationToken.None);

        result.Name.Should().Be("Admin");
        repoMock.Verify(r => r.AddAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRole_ShouldReturnNull_WhenNotFound()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Role?)null);

        var handler = new UpdateRoleCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new UpdateRoleCommand(99, MakeDto(99), "tester"), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteRole_ShouldReturnTrue_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var handler = new DeleteRoleCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeleteRoleCommand(1, "tester"), CancellationToken.None);

        result.Should().BeTrue();
    }
}