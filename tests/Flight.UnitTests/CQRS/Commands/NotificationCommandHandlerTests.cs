/************************************************************************************
* FICHIER : NotificationCommandHandlerTests.cs
* PROJET  : Flight.UnitTests
* DOSSIER : CQRS/Commands
*
* ROLE DU FICHIER
* -----------------------------------------------------------------------------
* Ce fichier contient les tests unitaires des handlers CQRS liés aux notifications.
*
* IMPORTANT
* -----------------------------------------------------------------------------
* Dans ce projet, il n'existe pas de :
*   - SendNotificationCommand
*   - NotificationCommandHandler
*   - Flight.Application.Interfaces.INotificationService
*
* La logique réelle de notification repose sur :
*   - CreateNotificationCommand / CreateNotificationCommandHandler
*   - UpdateNotificationCommand / UpdateNotificationCommandHandler
*   - DeleteNotificationCommand / DeleteNotificationCommandHandler
*
* Ces handlers dépendent de :
*   - IRepositoryManager              (accès aux repositories)
*   - IAuditTrailService              (journalisation / audit)
*
* Ce fichier est donc la version CORRECTE des tests, alignée sur le vrai code
* source du projet.
*
* Scénarios couverts :
* 1. Création d'une notification
* 2. Mise à jour d'une notification existante
* 3. Retour null si la notification à mettre à jour est introuvable
* 4. Suppression d'une notification existante
* 5. Retour false si la notification à supprimer est introuvable
*
* Technologies utilisées :
*   - xUnit
*   - Moq
*   - FluentAssertions
*
************************************************************************************/

using Flight.Application.CQRS.Commands.Notifications;
using Flight.Application.DTOs;
using Flight.Domain.Interfaces;
using Flight.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;
using NotificationEntity = Flight.Domain.Entities.Notification;

namespace Flight.UnitTests.CQRS.Commands;

/// <summary>
/// Tests unitaires des handlers CQRS de commande pour les notifications.
/// </summary>
public class NotificationCommandHandlerTests
{
    /// <summary>
    /// Construit un DTO Notification valide pour les tests.
    /// </summary>
    /// <param name="id">Identifiant de la notification.</param>
    /// <returns>Instance de <see cref="NotificationDto"/> prête à l'emploi.</returns>
    private static NotificationDto MakeDto(int id = 0)
    {
        return new NotificationDto(
            id: id,
            userId: 12,
            subject: "Information de vol",
            message: "Votre vol a été replanifié.",
            channel: "InApp",
            status: "Pending",
            createdAt: DateTime.UtcNow,
            sentAt: null
        );
    }

    /// <summary>
    /// Construit une entité Notification à partir du DTO de test.
    /// </summary>
    /// <param name="id">Identifiant à utiliser.</param>
    /// <returns>Entité Notification de domaine.</returns>
    private static NotificationEntity MakeEntity(int id = 1)
    {
        return MakeDto(id).ToEntity();
    }

    /// <summary>
    /// Prépare les mocks communs utilisés par les tests :
    /// - le repository de Notification
    /// - le manager de repositories
    /// </summary>
    /// <returns>
    /// Un tuple contenant :
    /// - le mock de <see cref="IRepositoryManager"/>
    /// - le mock de <see cref="IGenericRepository{T}"/> pour Notification
    /// </returns>
    private static (Mock<IRepositoryManager> managerMock, Mock<IGenericRepository<NotificationEntity>> repoMock) SetupMocks()
    {
        var repoMock = new Mock<IGenericRepository<NotificationEntity>>();
        var managerMock = new Mock<IRepositoryManager>();

        // Le handler utilise manager.Notification.
        managerMock.Setup(m => m.Notification).Returns(repoMock.Object);

        return (managerMock, repoMock);
    }

    /// <summary>
    /// Vérifie qu'une notification valide est correctement créée
    /// et qu'un DTO est bien renvoyé.
    /// </summary>
    [Fact]
    public async Task CreateNotification_ValidCommand_ShouldCreateAndReturnDto()
    {
        // Arrange
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.AddAsync(It.IsAny<NotificationEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new CreateNotificationCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new CreateNotificationCommand(MakeDto(), "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(12);
        result.Subject.Should().Be("Information de vol");
        result.Message.Should().Be("Votre vol a été replanifié.");

        repoMock.Verify(r => r.AddAsync(It.IsAny<NotificationEntity>()), Times.Once);

        auditMock.Verify(a => a.RecordAsync(
                "CREATE",
                "Notification",
                It.IsAny<string>(),
                It.IsAny<string>(),
                "unit-test",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Vérifie qu'une notification existante est correctement mise à jour.
    /// </summary>
    [Fact]
    public async Task UpdateNotification_ExistingNotification_ShouldUpdateAndReturnDto()
    {
        // Arrange
        var entity = MakeEntity(1);
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        var updatedDto = new NotificationDto(
            id: 1,
            userId: 99,
            subject: "Sujet mis à jour",
            message: "Message mis à jour",
            channel: "Email",
            status: "Sent",
            createdAt: entity.CreatedAt,
            sentAt: DateTime.UtcNow
        );

        repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
        repoMock.Setup(r => r.Update(It.IsAny<NotificationEntity>()))
                .Returns((Task<int>)Task.CompletedTask);

        auditMock.Setup(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var handler = new UpdateNotificationCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new UpdateNotificationCommand(1, updatedDto, "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.UserId.Should().Be(99);
        result.Subject.Should().Be("Sujet mis à jour");
        result.Message.Should().Be("Message mis à jour");
        result.Channel.Should().Be("Email");
        result.Status.Should().Be("Sent");

        repoMock.Verify(r => r.Update(It.IsAny<NotificationEntity>()), Times.Once);

        auditMock.Verify(a => a.RecordAsync(
                "UPDATE",
                "Notification",
                "1",
                It.IsAny<string>(),
                "unit-test",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Vérifie que le handler renvoie null lorsque la notification
    /// à mettre à jour n'existe pas.
    /// </summary>
    [Fact]
    public async Task UpdateNotification_NotFound_ShouldReturnNull()
    {
        // Arrange
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((NotificationEntity?)null);

        var handler = new UpdateNotificationCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new UpdateNotificationCommand(999, MakeDto(999), "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().BeNull();

        repoMock.Verify(r => r.Update(It.IsAny<NotificationEntity>()), Times.Never);
        auditMock.Verify(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    /// <summary>
    /// Vérifie qu'une notification existante est correctement supprimée.
    /// </summary>
    [Fact]
    public async Task DeleteNotification_ExistingNotification_ShouldReturnTrue()
    {
        // Arrange
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

        var handler = new DeleteNotificationCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new DeleteNotificationCommand(1, "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        repoMock.Verify(r => r.DeleteAsync(1), Times.Once);

        auditMock.Verify(a => a.RecordAsync(
                "DELETE",
                "Notification",
                "1",
                It.IsAny<string>(),
                "unit-test",
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Vérifie que le handler renvoie false si la notification
    /// à supprimer n'existe pas.
    /// </summary>
    [Fact]
    public async Task DeleteNotification_NotFound_ShouldReturnFalse()
    {
        // Arrange
        var (managerMock, repoMock) = SetupMocks();
        var auditMock = new Mock<IAuditTrailService>();

        repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((NotificationEntity?)null);

        var handler = new DeleteNotificationCommandHandler(managerMock.Object, auditMock.Object);

        // Act
        var result = await handler.Handle(
            new DeleteNotificationCommand(999, "unit-test"),
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        repoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        auditMock.Verify(a => a.RecordAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
