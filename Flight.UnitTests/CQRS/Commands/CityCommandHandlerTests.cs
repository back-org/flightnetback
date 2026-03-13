using Flight.Application.CQRS.Commands.Cities;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using CityEntity = Flight.Domain.Entities.City;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers CQRS de commande pour les villes.
/// </summary>
public class CityCommandHandlerTests
{
    /// <summary>
    /// Crée un DTO valide pour les tests.
    /// </summary>
    private static CityDto MakeDto(int id = 0)
    {
        return new CityDto(
            id,
            "Antananarivo",
            1,
            -18.8792,
            47.5079
        );
    }

    /// <summary>
    /// Crée une entité valide pour les tests.
    /// </summary>
    private static CityEntity MakeEntity(int id = 1)
    {
        return MakeDto(id).ToEntity();
    }

    /// <summary>
    /// Prépare les mocks du gestionnaire de repositories et du repository City.
    /// </summary>
    private static (Mock<IRepositoryManager> managerMock, Mock<IGenericRepository<CityEntity>> repoMock) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<CityEntity>>();
        var managerMock = new Mock<IRepositoryManager>();

        managerMock.Setup(m => m.City).Returns(repoMock.Object);

        return (managerMock, repoMock);
    }

    [Fact]
    public async Task CreateCity_ValidCommand_ShouldCreateAndReturnDto()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.AddAsync(It.IsAny<CityEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateCityCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new CreateCityCommand(MakeDto(), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Antananarivo");
    }

    [Fact]
    public async Task UpdateCity_ExistingCity_ShouldUpdateAndReturnDto()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.Update(It.IsAny<CityEntity>())).Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateCityCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateCityCommand(1, MakeDto(1), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateCity_NotFound_ShouldReturnNull()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((CityEntity?)null);

        var handler = new UpdateCityCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateCityCommand(999, MakeDto(999), "unit-test"),
            CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCity_ExistingCity_ShouldReturnTrue()
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

        var handler = new DeleteCityCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteCityCommand(1, "unit-test"),
            CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteCity_NotFound_ShouldReturnFalse()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((CityEntity?)null);

        var handler = new DeleteCityCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteCityCommand(999, "unit-test"),
            CancellationToken.None);

        result.Should().BeFalse();
    }
}