# ✈️ Flight Management System

## Présentation

**Flight Management System** est une application professionnelle de
gestion de vols développée avec **ASP.NET Core (.NET 8)** en suivant les
principes de **Clean Architecture**, **CQRS**, et **Domain Driven Design
(DDD)**.

L'objectif du projet est de fournir une plateforme **moderne, scalable
et maintenable** permettant de gérer :

-   les vols
-   les passagers
-   les équipes
-   les réservations
-   les notifications
-   l'audit des actions
-   les utilisateurs et rôles

Ce projet est conçu pour être **pédagogique et production‑ready**, avec
un code **entièrement commenté** pour être compréhensible même par un
développeur débutant.

------------------------------------------------------------------------

# 🧱 Architecture du projet

Le projet suit une **Clean Architecture** séparant les responsabilités.

    src/
     ├── Flight.Domain
     ├── Flight.Application
     ├── Flight.Infrastructure
     ├── Flight.API
     
    tests/
     ├── Flight.UnitTests

## 1️⃣ Domain

Le **Domain** contient le cœur métier.

Il inclut :

-   les **entités métier**
-   les **Value Objects**
-   les **interfaces de repository**
-   les **règles métier**

Exemples :

    Entities
    Flight
    Passenger
    Booking
    Notification
    User

------------------------------------------------------------------------

## 2️⃣ Application

La couche **Application** contient la logique applicative.

Elle implémente :

-   **CQRS**
-   **Commands**
-   **Queries**
-   **Handlers**
-   **DTO**
-   **Services applicatifs**

Exemple :

    Commands
    SendNotificationCommand

    Handlers
    NotificationCommandHandler

    DTO
    NotificationDto

------------------------------------------------------------------------

## 3️⃣ Infrastructure

La couche **Infrastructure** contient :

-   accès base de données
-   implémentation des repositories
-   email
-   notification
-   audit trail

Technologies utilisées :

-   Entity Framework Core
-   Logging
-   Cache mémoire
-   Services externes

------------------------------------------------------------------------

## 4️⃣ API

La couche **API** expose les fonctionnalités via :

-   **ASP.NET Core Web API**
-   **Swagger / OpenAPI**

Exemple d'endpoints :

    /api/flights
    /api/passengers
    /api/bookings
    /api/notifications
    /api/users

------------------------------------------------------------------------

## 5️⃣ Tests

Les tests sont situés dans :

    tests/Flight.UnitTests

Ils couvrent :

-   les handlers CQRS
-   les services applicatifs
-   les règles métier

Exemple :

    NotificationCommandHandlerTests

------------------------------------------------------------------------

# 🚀 Fonctionnalités principales

## Gestion des vols

-   création de vols
-   modification
-   suppression
-   recherche

## Gestion des passagers

-   enregistrement des passagers
-   historique des vols

## Gestion des réservations

-   réservation de sièges
-   annulation

## Notifications

Les utilisateurs peuvent recevoir des notifications :

-   confirmation de réservation
-   annulation de vol
-   nouveaux vols disponibles

Architecture :

    SendNotificationCommand
    NotificationCommandHandler
    INotificationService
    NotificationService

------------------------------------------------------------------------

# 🧪 Tests unitaires

Les tests utilisent :

-   **xUnit**
-   **Moq**
-   **FluentAssertions**

Exemple :

    NotificationCommandHandlerTests.cs

Les tests vérifient :

-   l'appel correct des services
-   la gestion des exceptions
-   le comportement attendu du handler

------------------------------------------------------------------------

# ⚙️ Installation

## 1 Installer .NET

Installer .NET SDK :

https://dotnet.microsoft.com

Vérifier :

    dotnet --version

------------------------------------------------------------------------

## 2 Cloner le projet

    git clone https://github.com/your-repository/flight-system.git

------------------------------------------------------------------------

## 3 Restaurer les packages

    dotnet restore

------------------------------------------------------------------------

## 4 Lancer l'application

    dotnet run --project src/Flight.API

------------------------------------------------------------------------

# 📦 Lancer les tests

    dotnet test

------------------------------------------------------------------------

# 📚 Bonnes pratiques appliquées

Ce projet respecte :

-   Clean Architecture
-   SOLID Principles
-   Dependency Injection
-   CQRS Pattern
-   Unit Testing
-   Documentation du code

------------------------------------------------------------------------

# 🛡 Sécurité

Les bonnes pratiques suivantes sont appliquées :

-   validation des entrées
-   gestion des exceptions
-   logging centralisé
-   audit trail

------------------------------------------------------------------------

# 👨‍💻 Contribution

Les contributions sont les bienvenues.

Avant de contribuer :

-   respecter les conventions du projet
-   ajouter des tests
-   documenter le code

------------------------------------------------------------------------

# 📄 Licence

MIT License

------------------------------------------------------------------------

# Auteur

Projet développé par :

**RANOELISON Dimbisoa Adrianno**

Expert Comptable\
Data Engineer\
Architecte IA & Data Platforms
