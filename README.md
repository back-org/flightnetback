# FlightNet API — Guide des fonctionnalités avancées

## Vue d'ensemble

FlightNet est une API REST .NET 9 de gestion de vols, réservations et passagers.
Ce document décrit les fonctionnalités avancées ajoutées à l'architecture de base.

---

## 📦 Structure du projet

```
FlightNet/
├── Flight.Api/                    # Couche présentation (Controllers, Middlewares, Auth)
│   ├── Authorization/             # Politiques d'autorisation fines
│   ├── Controllers/               # Contrôleurs versionnés
│   └── Middlewares/               # Exception handling, request logging
│
├── Flight.Application/            # Couche application (CQRS, Validators, Behaviors)
│   ├── CQRS/
│   │   ├── Commands/              # Commandes MediatR (Create, Update, Delete)
│   │   └── Queries/               # Requêtes MediatR (GetAll, GetById)
│   ├── Behaviors/                 # Pipeline MediatR (Validation, Logging)
│   └── Validators/                # Validateurs FluentValidation
│
├── Flight.Domain/                 # Entités, DTOs, interfaces domaine
├── Flight.Domain.Core/            # Abstractions (BaseEntity, AuditEntity...)
├── Flight.Infrastructure/         # EF Core, JWT, Repositories, AuditTrail
│   └── AuditTrail/               # Service et entité d'audit
│
├── Flight.UnitTests/              # Tests unitaires (xUnit + Moq + FluentAssertions)
│   ├── Validators/                # Tests des validateurs
│   └── CQRS/                     # Tests des handlers
│
├── Flight.IntegrationTests/       # Tests d'intégration (WebApplicationFactory)
├── .github/workflows/ci-cd.yml    # Pipeline CI/CD GitHub Actions
├── Dockerfile                     # Image Docker multi-stage
└── docker-compose.yml             # Stack complète (API + MySQL + Adminer)
```

---

## 🔍 FluentValidation

Chaque DTO dispose d'un validateur dédié dans `Flight.Application/Validators/` :

| Validateur | DTO | Règles principales |
|---|---|---|
| `FlightDtoValidator` | `FlightDto` | Code alphanumérique, départ > now, arrivée > départ, prix > 0, aéroports distincts |
| `BookingDtoValidator` | `BookingDto` | FlightId > 0, PassengerId > 0, enum valides |
| `PassengerDtoValidator` | `PassengerDto` | Email valide, contact téléphonique, longueurs |
| `AirlineDtoValidator` | `AirlineDto` | Nom non vide, état valide |
| `AirportDtoValidator` | `AirportDto` | Nom non vide, état valide |
| `LoginRequestValidator` | `LoginRequestModel` | UserName 3-50 chars, password 6+ chars |

Les validateurs sont intégrés dans le **pipeline MediatR** via `ValidationBehavior<T>`.
Toute erreur de validation lève une `ValidationException` avant l'exécution du handler.

---

## 🔄 Versioning d'API

L'API supporte trois modes de versioning simultanément :

```http
# URL segment (recommandé)
GET /api/v1/flights
GET /api/v2/flights

# Query string
GET /api/flights?api-version=1.0

# Header HTTP
GET /api/flights
X-API-Version: 1.0
```

Tous les contrôleurs sont annotés avec `[ApiVersion("1.0")]`.
La version par défaut est **v1.0** (assumée si non précisée).

---

## 🚦 Rate Limiting

Trois politiques de limitation sont configurées :

| Politique | Limite | Fenêtre | Utilisée sur |
|---|---|---|---|
| **Globale** (par IP) | 100 req | 1 minute | Toutes les routes |
| **auth** | 5 req | 5 minutes | `POST /account/login` |
| **write** | 30 req | 1 minute (sliding) | POST, PUT, DELETE |

En cas de dépassement, l'API répond `429 Too Many Requests` avec :
```json
{
  "error": "Trop de requêtes. Veuillez réessayer plus tard.",
  "retryAfter": 60
}
```

---

## ⚡ MediatR + CQRS

Les opérations sont séparées en **Commands** (écriture) et **Queries** (lecture) :

### Queries (lecture)
- `GetAllFlightsQuery` → Liste de tous les vols
- `GetFlightByIdQuery(int id)` → Vol par identifiant

### Commands (écriture)
- `CreateFlightCommand(FlightDto)` → Création + audit
- `UpdateFlightCommand(int, FlightDto)` → Mise à jour + audit
- `DeleteFlightCommand(int)` → Suppression + audit
- `CreateBookingCommand(BookingDto, string)` → Réservation + audit

### Pipeline Behaviors
1. **`LoggingBehavior<T>`** — Log automatique avec durée d'exécution
2. **`ValidationBehavior<T>`** — Validation FluentValidation avant chaque handler

---

## 🔐 Politiques d'autorisation fines

Quatre politiques définies dans `AuthorizationPolicies` :

| Politique | Condition |
|---|---|
| `ActiveAdmin` | Rôle Admin ET pas en session d'impersonation |
| `OwnerOrAdmin` | Rôle Admin OU propriétaire de la ressource |
| `WriteAccess` | Rôles Admin ou BasicUser |
| `AuthenticatedRead` | Tout utilisateur authentifié |

Exemple d'usage dans un contrôleur :
```csharp
[Authorize(Policy = AuthorizationPolicies.ActiveAdmin)]
[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id) { ... }
```

---

## 📋 Audit Trail

Toutes les opérations sensibles sont enregistrées dans la table `AuditLogs` :

| Champ | Description |
|---|---|
| `Action` | CREATE, UPDATE, DELETE, LOGIN, LOGIN_FAILED, LOGOUT, IMPERSONATION_START... |
| `EntityName` | Nom de l'entité (Flight, Booking, Account...) |
| `EntityId` | Identifiant de l'entité |
| `Details` | Détails de l'opération |
| `PerformedBy` | Nom de l'utilisateur |
| `IpAddress` | IP de l'appelant |
| `PerformedAt` | Timestamp UTC |

> **Note :** L'audit est non-bloquant — une erreur d'audit ne fait jamais échouer l'opération principale.

---

## 🧪 Tests

### Tests unitaires (`Flight.UnitTests`)
```bash
dotnet test Flight.UnitTests/Flight.UnitTests.csproj -c Release
```

Couvre :
- Tous les validateurs FluentValidation (cas valides et invalides)
- Handlers CQRS (Create, Update, Delete) avec Moq

### Tests d'intégration (`Flight.IntegrationTests`)
```bash
dotnet test Flight.IntegrationTests/Flight.IntegrationTests.csproj -c Release
```

Utilise `WebApplicationFactory<Program>` avec une base InMemory.
Couvre les endpoints HTTP (statuts, pagination, authentification).

---

## 🐳 Docker

### Build et démarrage rapide
```bash
# Copier les variables d'environnement
cp .env.example .env

# Démarrer la stack complète (API + MySQL)
docker compose up -d

# Avec Adminer (interface DB)
docker compose --profile dev up -d
```

### Endpoints disponibles
- API : http://localhost:8080
- Documentation Scalar : http://localhost:8080/scalar/v1
- Health check : http://localhost:8080/health
- Adminer (profil dev) : http://localhost:8888

---

## 🚀 CI/CD GitHub Actions

Le pipeline `.github/workflows/ci-cd.yml` exécute 4 jobs :

1. **Build & Test** — Build, tests unitaires + intégration, couverture de code
2. **Security Scan** — Vulnérabilités NuGet + OWASP Dependency Check
3. **Docker Build & Push** — Image vers GitHub Container Registry (sur main/develop)
4. **Deploy Production** — Déploiement SSH (sur main uniquement, avec approbation)

### Secrets GitHub requis
```
PROD_HOST       — Hôte du serveur de production
PROD_USER       — Utilisateur SSH
PROD_SSH_KEY    — Clé SSH privée
```

---

## 🔧 Migration base de données

```bash
# Appliquer toutes les migrations (incluant AuditLogs)
dotnet ef database update --project Flight.Infrastructure --startup-project Flight.Api

# Créer une nouvelle migration
dotnet ef migrations add NomMigration --project Flight.Infrastructure --startup-project Flight.Api
```
