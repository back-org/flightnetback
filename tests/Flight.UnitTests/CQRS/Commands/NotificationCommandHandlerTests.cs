/************************************************************************************
* FICHIER : NotificationCommandHandlerTests.cs
* PROJET  : Flight.UnitTests
* DOSSIER : CQRS/Commands
*
* ROLE DU FICHIER
* -----------------------------------------------------------------------------
* Ce fichier contient les tests unitaires pour le NotificationCommandHandler.
*
* Le NotificationCommandHandler est responsable de :
*    - recevoir une commande d'envoi de notification
*    - appeler le service de notification
*    - gérer les erreurs éventuelles
*
* Les tests permettent de vérifier :
*
* 1. que la notification est envoyée correctement
* 2. que le handler appelle bien le service de notification
* 3. que les erreurs sont gérées correctement
*
* Technologies utilisées :
*
* - xUnit           : Framework de tests unitaires
* - Moq             : Simulation (mock) des dépendances
* - FluentAssertions: Assertions lisibles
*
* Pourquoi tester ce Handler ?
*
* Dans une architecture CQRS, les Handlers sont le coeur de la logique
* applicative. Les tester garantit que les commandes exécutent bien
* les actions attendues.
*
************************************************************************************/

using System;
using System.Threading;
using System.Threading.Tasks;

using Xunit;
using Moq;
using FluentAssertions;

using Flight.Application.CQRS.Commands.Notifications;
using Flight.Application.Interfaces;
using Flight.Domain.Entities;

namespace Flight.UnitTests.CQRS.Commands
{
    /***************************************************************************
    * CLASSE : NotificationCommandHandlerTests
    *
    * ROLE
    * -------------------------------------------------------------------------
    * Cette classe contient l'ensemble des tests unitaires concernant
    * le NotificationCommandHandler.
    *
    * Chaque méthode test correspond à un scénario métier précis.
    *
    ***************************************************************************/
    public class NotificationCommandHandlerTests
    {
        /// <summary>
        /// Mock du service de notification.
        /// Il simule le comportement du vrai service sans dépendre
        /// d'une infrastructure réelle (email, push, etc.).
        /// </summary>
        private readonly Mock<INotificationService> _notificationServiceMock;

        /// <summary>
        /// Instance du handler à tester.
        /// </summary>
        private readonly NotificationCommandHandler _handler;

        /// <summary>
        /// Constructeur de la classe de tests.
        /// Initialise les mocks et les dépendances.
        /// </summary>
        public NotificationCommandHandlerTests()
        {
            // Création du mock du service
            _notificationServiceMock = new Mock<INotificationService>();

            // Injection du mock dans le handler
            _handler = new NotificationCommandHandler(_notificationServiceMock.Object);
        }

        /***************************************************************************
        * TEST 1
        *
        * Vérifie que le handler appelle bien le service de notification.
        ***************************************************************************/
        [Fact]
        public async Task Handle_Should_Send_Notification()
        {
            // ------------------------------------------------------------------
            // ARRANGE
            // Préparation des données du test
            // ------------------------------------------------------------------

            var command = new SendNotificationCommand
            (
                userId: Guid.NewGuid(),
                title: "Nouveau vol",
                message: "Un nouveau vol est disponible."
            );

            // Simulation du comportement du service
            _notificationServiceMock
                .Setup(service => service.SendAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // ------------------------------------------------------------------
            // ACT
            // Exécution du handler
            // ------------------------------------------------------------------

            await _handler.Handle(command, CancellationToken.None);

            // ------------------------------------------------------------------
            // ASSERT
            // Vérifie que le service a été appelé exactement une fois
            // ------------------------------------------------------------------

            _notificationServiceMock.Verify(service =>
                service.SendAsync(
                    command.UserId,
                    command.Title,
                    command.Message),
                Times.Once);
        }

        /***************************************************************************
        * TEST 2
        *
        * Vérifie que le handler ne lance pas d'exception lorsque tout se passe bien.
        ***************************************************************************/
        [Fact]
        public async Task Handle_Should_Not_Throw_Exception_When_Service_Works()
        {
            // ------------------------------------------------------------------
            // ARRANGE
            // ------------------------------------------------------------------

            var command = new SendNotificationCommand
            (
                Guid.NewGuid(),
                "Test Notification",
                "Message de test"
            );

            _notificationServiceMock
                .Setup(x => x.SendAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // ------------------------------------------------------------------
            // ACT
            // ------------------------------------------------------------------

            Func<Task> act = async () =>
                await _handler.Handle(command, CancellationToken.None);

            // ------------------------------------------------------------------
            // ASSERT
            // ------------------------------------------------------------------

            await act.Should().NotThrowAsync();
        }

        /***************************************************************************
        * TEST 3
        *
        * Vérifie que le handler propage correctement les exceptions.
        ***************************************************************************/
        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Service_Fails()
        {
            // ------------------------------------------------------------------
            // ARRANGE
            // ------------------------------------------------------------------

            var command = new SendNotificationCommand
            (
                Guid.NewGuid(),
                "Erreur",
                "Test erreur"
            );

            _notificationServiceMock
                .Setup(x => x.SendAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Erreur d'envoi"));

            // ------------------------------------------------------------------
            // ACT
            // ------------------------------------------------------------------

            Func<Task> act = async () =>
                await _handler.Handle(command, CancellationToken.None);

            // ------------------------------------------------------------------
            // ASSERT
            // ------------------------------------------------------------------

            await act.Should().ThrowAsync<Exception>();
        }
    }
}