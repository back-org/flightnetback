# ✈️ Flight Management System

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue)
![Pattern](https://img.shields.io/badge/Pattern-CQRS-green)
![Tests](https://img.shields.io/badge/Tested%20with-xUnit-orange)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

------------------------------------------------------------------------

# Overview

**Flight Management System** is an enterprise-grade backend platform
designed to manage airline operations.\
The project demonstrates a **production-level architecture** built with
modern backend standards.

The system uses:

-   **ASP.NET Core (.NET 8)**
-   **Clean Architecture**
-   **CQRS**
-   **Domain Driven Design**
-   **Unit Testing**
-   **Modular Infrastructure**

The goal of the project is to provide a **scalable, maintainable and
extensible backend** suitable for real-world airline systems.

------------------------------------------------------------------------

# Key Features

The platform supports the management of:

-   ✈️ Flights
-   👤 Passengers
-   🎫 Reservations
-   👨‍✈️ Crew and Teams
-   🔔 Notifications
-   📝 Audit Logs
-   🔐 Users and Roles

------------------------------------------------------------------------

# Architecture

The system follows **Clean Architecture principles**.

                    +--------------------------+
                    |        API Layer         |
                    | Controllers / Swagger    |
                    +------------+-------------+
                                 |
                                 v
                    +--------------------------+
                    |     Application Layer     |
                    | CQRS / Handlers / DTOs   |
                    +------------+-------------+
                                 |
                                 v
                    +--------------------------+
                    |       Domain Layer       |
                    | Entities / Rules / DDD   |
                    +------------+-------------+
                                 |
                                 v
                    +--------------------------+
                    |    Infrastructure Layer  |
                    | EF Core / Services       |
                    +--------------------------+

------------------------------------------------------------------------

# CQRS Flow Example

Notification workflow:

    API Controller
          │
          ▼
    SendNotificationCommand
          │
          ▼
    NotificationCommandHandler
          │
          ▼
    INotificationService
          │
          ▼
    NotificationService

------------------------------------------------------------------------

# Project Structure

    src
     ├── Flight.Domain
     │     ├── Entities
     │     ├── ValueObjects
     │     └── Interfaces
     │
     ├── Flight.Application
     │     ├── Commands
     │     ├── Queries
     │     ├── Handlers
     │     ├── DTOs
     │     └── Services
     │
     ├── Flight.Infrastructure
     │     ├── Persistence
     │     ├── Repositories
     │     ├── Notifications
     │     └── AuditTrail
     │
     └── Flight.API
           ├── Controllers
           └── Program.cs

    tests
     └── Flight.UnitTests
           ├── Commands
           ├── Queries
           └── Services

------------------------------------------------------------------------

# Core Technologies

  Technology              Purpose
  ----------------------- -------------------
  ASP.NET Core            Web API
  Entity Framework Core   ORM
  MediatR                 CQRS
  xUnit                   Unit testing
  Moq                     Mocking
  FluentAssertions        Assertions
  Swagger                 API documentation

------------------------------------------------------------------------

# Database Architecture (Concept)

    Users
      │
      ├── Reservations
      │        │
      │        └── Flights
      │
      └── Notifications

------------------------------------------------------------------------

# Notification Module

The notification module sends alerts to users for:

-   reservation confirmations
-   flight cancellations
-   system announcements

Components:

    SendNotificationCommand
    NotificationCommandHandler
    INotificationService
    NotificationService
    NotificationDto

------------------------------------------------------------------------

# Testing Strategy

Tests are located in:

    tests/Flight.UnitTests

Example test file:

    NotificationCommandHandlerTests.cs

Testing stack:

-   **xUnit**
-   **Moq**
-   **FluentAssertions**

Tests verify:

-   correct handler behaviour
-   service invocation
-   exception management

------------------------------------------------------------------------

# Installation

## Install .NET

Download .NET SDK:

https://dotnet.microsoft.com

Verify installation:

    dotnet --version

------------------------------------------------------------------------

## Clone the repository

    git clone https://github.com/your-repository/flight-management-system.git

------------------------------------------------------------------------

## Restore dependencies

    dotnet restore

------------------------------------------------------------------------

## Run the API

    dotnet run --project src/Flight.API

Swagger UI:

    http://localhost:5000/swagger

------------------------------------------------------------------------

# Running Tests

    dotnet test

------------------------------------------------------------------------

# CI/CD Example (GitHub Actions)

Example pipeline:

    name: .NET Build and Test

    on:
      push:
        branches: [ main ]

    jobs:
      build:

        runs-on: ubuntu-latest

        steps:
          - uses: actions/checkout@v4

          - name: Setup .NET
            uses: actions/setup-dotnet@v4
            with:
              dotnet-version: 8.0.x

          - name: Restore
            run: dotnet restore

          - name: Build
            run: dotnet build --configuration Release

          - name: Test
            run: dotnet test

------------------------------------------------------------------------

# Security

The application applies security best practices:

-   dependency injection
-   input validation
-   centralized logging
-   audit trail
-   exception handling

------------------------------------------------------------------------

# Roadmap

Future improvements may include:

-   SignalR real-time notifications
-   Redis distributed caching
-   microservices architecture
-   event-driven messaging (Kafka / RabbitMQ)
-   containerization with Docker

------------------------------------------------------------------------

# Contribution

Contributions are welcome.

Please ensure:

-   code follows Clean Architecture
-   tests are added
-   documentation is updated

------------------------------------------------------------------------

# Author

**RANOELISON Dimbisoa Adrianno**

Data Engineer\
AI & Data Architect\
Expert Comptable
