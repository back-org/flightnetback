# FlightNet API

FlightNet API est une API REST moderne développée avec **ASP.NET Core 9**, **Entity Framework Core**, **JWT Authentication** et une architecture modulaire orientée maintenabilité.

## Objectifs du projet

Cette API permet de gérer les principales ressources d’un système de réservation aérienne :

- vols
- compagnies aériennes
- aéroports
- villes
- pays
- passagers
- réservations
- véhicules

Le projet est conçu pour être :

- maintenable
- testable
- évolutif
- documenté
- prêt pour un usage professionnel

## Stack technique

- ASP.NET Core 9
- Entity Framework Core
- MySQL / MariaDB via Pomelo
- JWT Authentication
- Scalar / OpenAPI
- Serilog
- Newtonsoft.Json

## Architecture

Le projet est organisé en plusieurs couches :

- `Flight.Api` : exposition HTTP, contrôleurs, middlewares, configuration
- `Flight.Application` : configuration applicative, services transverses
- `Flight.Domain` : entités métier, interfaces, objets de transfert
- `Flight.Infrastructure` : persistance, repositories, authentification JWT
- `Flight.CrossCutting` : composants techniques transverses
- `Flight.Util` : constantes et utilitaires

## Fonctionnalités principales

- authentification JWT
- gestion des refresh tokens
- CRUD sur les entités métier
- pagination
- réponses API standardisées
- logs structurés
- middleware global de gestion d’erreurs
- endpoint de santé `/health`
- documentation interactive OpenAPI avec Scalar

## Lancement local

### 1. Restaurer les packages

```bash
dotnet restore