using Flight.Application.CQRS.Commands.Countries;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using CountryEntity = Flight.Domain.Entities.Country;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers CQRS liés aux pays.
/// </summary>
public class CountryCommandHandlerTests
{
    /// <summary>
    /// Construit un DTO valide pour les tests.
    /// </summary>
    private static CountryDto MakeDto(int id = 0)
    {
        return new CountryDto(
            id,
            "Madagascar",
            "MG",
            "MDG"
        );
    }

    /// <summary>
    /// Construit une entité Country valide.
    /// </summary>
    private static CountryEntity MakeEntity(int id = 1)
    {
        return MakeDto(id).ToEntity();
    }

    /// <summary>
    /// Initialise les mocks nécessaires.
    /// </summary>
    private static (Mock<IRepositoryManager>, Mock<IGenericRepository<CountryEntity>>) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<CountryEntity>>();
        var managerMock = new Mock<IRepositoryManager>();

        managerMock.Setup(m => m.Country).Returns(repoMock.Object);

        return (managerMock, repoMock);
    }

    /// <summary>
    /// Vérifie la création d'un pays.
    /// </summary>
    [Fact]
    public async Task CreateCountry_ShouldCreateEntity()
    {
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.AddAsync(It.IsAny<CountryEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        var handler = new CreateCountryCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new CreateCountryCommand(MakeDto(), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        result.Name.Should().Be("Madagascar");

        repoMock.Verify(r => r.AddAsync(It.IsAny<CountryEntity>()), Times.Once);
    }

    /// <summary>
    /// Vérifie la mise à jour d'un pays existant.
    /// </summary>
    [Fact]
    public async Task UpdateCountry_ShouldUpdateEntity()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(entity);

        repoMock.Setup(r => r.Update(It.IsAny<CountryEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        var handler = new UpdateCountryCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new UpdateCountryCommand(1, MakeDto(1), "unit-test"),
            CancellationToken.None);

        result.Should().NotBeNull();
        repoMock.Verify(r => r.Update(It.IsAny<CountryEntity>()), Times.Once);
    }

    /// <summary>
    /// Vérifie la suppression d'un pays existant.
    /// </summary>
    [Fact]
    public async Task DeleteCountry_ShouldDeleteEntity()
    {
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(entity);

        repoMock.Setup(r => r.DeleteAsync(1))
                .Returns((Task<int>)Task.CompletedTask);

        var handler = new DeleteCountryCommandHandler(managerMock.Object, auditMock.Object);

        var result = await handler.Handle(
            new DeleteCountryCommand(1, "unit-test"),
            CancellationToken.None);

        result.Should().BeTrue();
    }
}