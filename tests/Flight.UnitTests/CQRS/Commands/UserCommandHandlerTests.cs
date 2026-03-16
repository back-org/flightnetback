using Flight.Application.CQRS.Commands.Users;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers de commandes pour les utilisateurs.
/// </summary>
public class UserCommandHandlerTests
{
    private static User MakeEntity(int id = 1) => new()
    {
        Id = id,
        UserName = "patrick",
        Email = "patrick@example.com",
        FirstName = "Patrick",
        LastName = "Ranoelison",
        PhoneNumber = "0320000000",
        IsActive = true,
        PasswordHash = "hashed",
        CreatedAt = DateTime.UtcNow
    };

    private static UserDto MakeDto(int id = 0) => new(
        id,
        "patrick",
        "patrick@example.com",
        "Patrick",
        "Ranoelison",
        "0320000000",
        true,
        DateTime.UtcNow,
        null,
        null);

    private static (
        Mock<IRepositoryManager> managerMock,
        Mock<IGenericRepository<User>> repoMock,
        Mock<IAuditTrailService> auditMock) Setup()
    {
        var repoMock = new Mock<IGenericRepository<User>>();
        var managerMock = new Mock<IRepositoryManager>();
        var auditMock = new Mock<IAuditTrailService>();

        managerMock.Setup(m => m.User).Returns(repoMock.Object);

        return (managerMock, repoMock, auditMock);
    }

    [Fact]
    public async Task CreateUser_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(1);

        var handler = new CreateUserCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new CreateUserCommand(MakeDto(), "tester"), CancellationToken.None);

        result.Should().NotBeNull();
        result.UserName.Should().Be("patrick");
        repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnNull_WhenNotFound()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        var handler = new UpdateUserCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new UpdateUserCommand(99, MakeDto(99), "tester"), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateUser_ShouldUpdate_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.Update(It.IsAny<User>())).ReturnsAsync(1);

        var handler = new UpdateUserCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new UpdateUserCommand(1, MakeDto(1), "tester"), CancellationToken.None);

        result.Should().NotBeNull();
        repoMock.Verify(r => r.Update(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnFalse_WhenNotFound()
    {
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync((User?)null);

        var handler = new DeleteUserCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeleteUserCommand(10, "tester"), CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnTrue_WhenFound()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock, auditMock) = Setup();
        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);

        var handler = new DeleteUserCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(new DeleteUserCommand(1, "tester"), CancellationToken.None);

        result.Should().BeTrue();
        repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}