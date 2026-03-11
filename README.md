# ✈️ FlightNet — Backend API REST ASP.NET Core

<div align="center">

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)
![Entity Framework](https://img.shields.io/badge/EF%20Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-Auth-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-19-DD0031?style=for-the-badge&logo=angular&logoColor=white)

**API REST complète pour une application de gestion de vols et de réservations.**  
Architecture propre (Clean Architecture) · JWT Authentication · Scalar API Docs

</div>

---

## 📑 Table des matières

- [À propos du projet](#-à-propos-du-projet)
- [Architecture](#-architecture)
- [Technologies](#-technologies)
- [Prérequis](#-prérequis)
- [Installation & Démarrage](#-installation--démarrage)
- [Configuration](#-configuration)
- [Endpoints API](#-endpoints-api)
- [Authentification](#-authentification-jwt)
- [Structure du projet](#-structure-du-projet)
- [Tests](#-tests)
- [Contribuer](#-contribuer)

---

## 🧭 À propos du projet

**FlightNet Backend** est une API REST développée avec **ASP.NET Core 9** servant de backend pour une application Angular de gestion de vols. Elle expose des endpoints CRUD complets pour :

- 🛫 **Vols** — planification, horaires, capacités, prix
- 🏢 **Compagnies aériennes** — gestion des opérateurs
- 🏛️ **Aéroports** — points de départ et d'arrivée
- 🎫 **Réservations** — création et suivi des bookings passager
- 👤 **Passagers** — profils et informations de contact
- 🚗 **Véhicules** — transport terrestre associé
- 🌍 **Pays & Villes** — référentiel géographique

---

## 🏛️ Architecture

Ce projet suit les principes de la **Clean Architecture** (Architecture en couches) :

```
┌─────────────────────────────────────────────┐
│              Flight.Api                      │  ← Présentation (Controllers, Program.cs)
├─────────────────────────────────────────────┤
│           Flight.Application                 │  ← Middlewares, Services, Configuration
├─────────────────────────────────────────────┤
│            Flight.Domain                     │  ← Entités, DTOs, Interfaces de dépôt
├─────────────────────────────────────────────┤
│          Flight.Domain.Core                  │  ← Abstractions, BaseEntity, Specs, Notifics
├─────────────────────────────────────────────┤
│         Flight.Infrastructure                │  ← EF Core, Repositories, JWT, Logs
├─────────────────────────────────────────────┤
│          Flight.CrossCutting                 │  ← Utilitaires transversaux (Assembly)
├─────────────────────────────────────────────┤
│            Flight.Util                       │  ← Constantes partagées
└─────────────────────────────────────────────┘
```

**Patterns utilisés :**
- **Repository Pattern** — abstraction de l'accès aux données via `IGenericRepository<T>`
- **Repository Manager** — accès centralisé à tous les dépôts via `IRepositoryManager`
- **Service Manager** — couche service dédiée via `IServiceManager`
- **DTO (Data Transfer Object)** — séparation entités/contrats API avec des records C#
- **Result Pattern** — retours typés avec `Result<T>` basé sur Flunt.Notifications
- **Specification Pattern** — requêtes réutilisables avec `BaseSpecification<T>`

---

## 🛠️ Technologies

| Technologie | Version | Rôle |
|---|---|---|
| ASP.NET Core | 9.0 | Framework web API |
| Entity Framework Core | 9.0 | ORM base de données |
| MySQL (Pomelo) | 9.0 | Base de données relationnelle |
| JWT Bearer | - | Authentification stateless |
| Scalar | 2.x | Documentation API interactive |
| NLog | 5.x | Logging structuré |
| Flunt | - | Notification / Result Pattern |
| Newtonsoft.Json | 13.x | Sérialisation JSON |
| ASP.NET Core Identity | 9.0 | Gestion d'identité (optionnel) |

---

## ✅ Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MySQL 8.0+](https://dev.mysql.com/downloads/) ou MariaDB 10.6+
- [Git](https://git-scm.com/)

---

## 🚀 Installation & Démarrage

### 1. Cloner le dépôt

```bash
git clone https://github.com/votre-compte/flightnetback.git
cd flightnetback
```

### 2. Configurer la base de données

Créez la base de données MySQL :

```sql
CREATE DATABASE flights CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 3. Configurer les variables d'environnement

Copiez et adaptez le fichier de configuration :

```bash
cp Flight.Api/appsettings.json Flight.Api/appsettings.Development.json
```

Modifiez `appsettings.Development.json` avec vos valeurs :

```json
{
  "ConnectionStrings": {
    "DbConn": "server=127.0.0.1;port=3306;user=root;password=VOTRE_MOT_DE_PASSE;database=flights"
  },
  "jwtTokenConfig": {
    "secret": "VOTRE_CLÉ_SECRÈTE_LONGUE_D'AU_MOINS_32_CARACTÈRES",
    "issuer": "FlightNetApi",
    "audience": "FlightNetClient",
    "accessTokenExpiration": 60,
    "refreshTokenExpiration": 1440
  }
}
```

> ⚠️ **Sécurité** : Ne committez jamais vos secrets dans Git. Utilisez des variables d'environnement en production.

### 4. Appliquer les migrations

```bash
cd Flight.Api
dotnet ef database update --project ../Flight.Infrastructure
```

### 5. Lancer l'API

```bash
dotnet run --project Flight.Api
```

L'API sera disponible sur :
- **HTTPS** : `https://localhost:7xxx`
- **Scalar Docs** : `https://localhost:7xxx/scalar/v1`

---

## ⚙️ Configuration

### Variables d'environnement (production)

| Variable | Description |
|---|---|
| `DB_CONNECTION_STRING` | Chaîne de connexion MySQL complète |
| `ASPNETCORE_ENVIRONMENT` | `Development`, `Staging` ou `Production` |

### Section `jwtTokenConfig`

| Clé | Type | Description |
|---|---|---|
| `secret` | string | Clé HMAC-SHA256 (min. 32 caractères) |
| `issuer` | string | Émetteur du token (claim `iss`) |
| `audience` | string | Audience du token (claim `aud`) |
| `accessTokenExpiration` | int | Durée du token d'accès (minutes) |
| `refreshTokenExpiration` | int | Durée du refresh token (minutes) |

---

## 📡 Endpoints API

La documentation interactive complète est disponible via **Scalar** en mode développement à `/scalar/v1`.

### Authentification

| Méthode | Endpoint | Description | Auth |
|---|---|---|---|
| `POST` | `/api/account/login` | Connexion utilisateur | Public |
| `POST` | `/api/account/logout` | Déconnexion | 🔒 Bearer |
| `POST` | `/api/account/refresh-token` | Renouvellement du token | 🔒 Bearer |
| `GET` | `/api/account/user` | Utilisateur courant | 🔒 Bearer |
| `POST` | `/api/account/impersonation` | Usurper une identité | 🔒 Admin |
| `POST` | `/api/account/stop-impersonation` | Arrêter l'usurpation | 🔒 Bearer |

### Ressources (CRUD)

Chaque ressource expose 5 endpoints standardisés :

| Méthode | Endpoint | Description | Auth |
|---|---|---|---|
| `GET` | `/api/{resource}` | Liste complète | Public |
| `GET` | `/api/{resource}/{id}` | Par identifiant | Public |
| `POST` | `/api/{resource}` | Création | 🔒 BasicUser/Admin |
| `PUT` | `/api/{resource}` | Mise à jour | 🔒 BasicUser/Admin |
| `DELETE` | `/api/{resource}/{id}` | Suppression | 🔒 Admin |

**Ressources disponibles :** `airlines`, `airports`, `flights`, `bookings`, `passengers`, `vehicles`, `countries`, `cities`

---

## 🔐 Authentification JWT

L'API utilise une authentification par **JWT Bearer Token** avec refresh token.

### Flux d'authentification

```
Client                          API
  │                              │
  │  POST /api/account/login     │
  │──────────────────────────────▶│
  │                              │  Valide identifiants
  │  { accessToken, refreshToken }│
  │◀──────────────────────────────│
  │                              │
  │  GET /api/flights            │
  │  Authorization: Bearer <token>│
  │──────────────────────────────▶│
  │                              │  Valide JWT
  │  [{ id, code, ... }]         │
  │◀──────────────────────────────│
  │                              │
  │  POST /api/account/refresh-token  
  │──────────────────────────────▶│
  │  { nouveaux tokens }         │
  │◀──────────────────────────────│
```

### Utilisateurs de test (développement)

| Utilisateur | Mot de passe | Rôle |
|---|---|---|
| `test1` | `password1` | BasicUser |
| `test2` | `password2` | BasicUser |
| `admin` | `securePassword` | Admin |

> ⚠️ Remplacez ces comptes par une vraie source de données en production.

---

## 📁 Structure du projet

```
flightnetback/
├── Flight.Api/                      # Projet principal (ASP.NET Web API)
│   ├── Controllers/                 # Contrôleurs REST
│   │   ├── AccountController.cs     # Auth JWT (login, logout, refresh, impersonation)
│   │   ├── AirlinesController.cs    # CRUD compagnies aériennes
│   │   ├── AirportsController.cs    # CRUD aéroports
│   │   ├── BookingsController.cs    # CRUD réservations
│   │   ├── CitiesController.cs      # CRUD villes
│   │   ├── CountriesController.cs   # CRUD pays
│   │   ├── FlightsController.cs     # CRUD vols
│   │   ├── PassengersController.cs  # CRUD passagers
│   │   ├── VehiclesController.cs    # CRUD véhicules
│   │   └── ParentController.cs      # Contrôleur de base
│   ├── appsettings.json             # Configuration (sans secrets)
│   └── Program.cs                   # Point d'entrée et pipeline HTTP
│
├── Flight.Application/              # Couche application (middlewares DI)
│   ├── Applications/
│   │   ├── AuthMiddleware.cs        # Configuration ASP.NET Identity
│   │   ├── CacheMiddleware.cs       # Configuration du cache de réponse
│   │   ├── CorsMiddleware.cs        # Configuration CORS
│   │   ├── DatabaseMiddleware.cs    # Configuration EF Core / MySQL
│   │   ├── JwtMiddleware.cs         # Configuration JWT Bearer
│   │   ├── LifetimeMiddleware.cs    # Enregistrement DI (repos, services)
│   │   ├── SerializationMiddleware.cs # Configuration JSON
│   │   └── UsersService.cs          # Service utilisateur + rôles
│   └── Concrete/
│       ├── ConfigManager.cs         # Lecture appsettings.json
│       └── RepositoryExtensions.cs  # Extensions de requêtes (à étendre)
│
├── Flight.Domain/                   # Couche domaine (entités, contrats)
│   ├── Entities/                    # Entités EF Core + DTOs + Extensions
│   │   ├── Airline.cs, Airport.cs, Booking.cs
│   │   ├── City.cs, Country.cs, Flight.cs
│   │   ├── Passenger.cs, Vehicle.cs
│   │   └── _constants.cs            # Enums (Genre, Confort, Statut, State)
│   ├── Interfaces/
│   │   ├── IGenericRepository.cs    # Contrat dépôt générique
│   │   └── INotificationRepository.cs # Contrat avec Result Pattern
│   └── Results/
│       └── Result.cs                # Classes Result / Result<T>
│
├── Flight.Domain.Core/              # Abstractions de base réutilisables
│   ├── Abstracts/
│   │   ├── AuditEntity.cs           # Entité avec audit (créé/modifié par)
│   │   ├── BaseEntity.cs            # Entité de base avec ID + dates
│   │   ├── DeleteEntity.cs          # Soft-delete (IsActive, IsDeleted)
│   │   └── EntityBase.cs            # Base avec Notifiable<T>
│   ├── Attributes/
│   │   └── UpdateGreaterThanCreateAttribute.cs # Validation date update > création
│   ├── Interfaces/
│   │   ├── IAuditEntity.cs, IDeleteEntity.cs, IEntityBase.cs
│   │   └── ISpecifications.cs       # Contrat Specification Pattern
│   ├── Models/
│   │   ├── BaseModel.cs, ResponseModel.cs
│   └── Specifications/
│       └── BaseSpecification.cs     # Implémentation Specification Pattern
│
├── Flight.Infrastructure/           # Couche infrastructure (EF, Auth, Logs)
│   ├── Auth/
│   │   ├── JwtAuthManager.cs        # Génération/validation/révocation JWT
│   │   ├── JwtRefreshTokenCache.cs  # Nettoyage périodique des tokens
│   │   └── JwtTokenConfig.cs        # DTO de configuration JWT
│   ├── Contracts/
│   │   ├── GenericRepository.cs     # Implémentation CRUD générique EF Core
│   │   └── RepositoryManager.cs     # Gestionnaire centralisé des dépôts
│   ├── Database/
│   │   └── FlightContext.cs         # DbContext EF Core (IdentityDbContext)
│   ├── EntityConfigurations/
│   │   └── CountryConfiguration.cs  # Fluent API pour Country
│   ├── Implementations/
│   │   ├── LoggerManager.cs         # Implémentation NLog
│   │   ├── Service.cs               # Service générique de base
│   │   └── ServiceManager.cs        # Gestionnaire des services
│   ├── Interfaces/
│   │   ├── ILoggerManager.cs        # Interface logger
│   │   ├── IRepositoryManager.cs    # Interface + classes dépôts concrets
│   │   ├── IService.cs              # Interface service générique
│   │   └── IServiceManager.cs       # Interface + classes services concrets
│   └── Migrations/                  # Migrations EF Core
│
├── Flight.CrossCutting/             # Utilitaires transversaux
│   └── Assemblies/AssemblyUtil.cs   # Enumération des assemblies du projet
│
├── Flight.Util/                     # Constantes partagées
│   └── Constants.cs
│
└── Flight.UnitTests/                # Tests unitaires (à compléter)
    └── Constants.cs
```

---

## 🧪 Tests

Le projet contient un projet de tests `Flight.UnitTests`. Pour lancer les tests :

```bash
dotnet test
```

> Les tests unitaires sont à compléter. Il est recommandé de tester :
> - Les contrôleurs (xUnit + Moq)
> - Les validations des DTOs
> - Le `JwtAuthManager`
> - Le `GenericRepository` avec un contexte en mémoire (`UseInMemoryDatabase`)

---

## 🤝 Contribuer

1. Forkez le dépôt
2. Créez votre branche : `git checkout -b feature/ma-fonctionnalite`
3. Committez vos changements : `git commit -m 'feat: ajouter X'`
4. Poussez la branche : `git push origin feature/ma-fonctionnalite`
5. Ouvrez une Pull Request

### Conventions de code

- Commentaires XML (`///`) obligatoires sur toutes les méthodes, propriétés et classes publiques
- Nommage en anglais pour le code, en français pour les commentaires XML
- Retourner des objets JSON (pas des chaînes), utiliser les codes HTTP appropriés (`201 Created`, `204 NoContent`, etc.)
- Aucun identifiant de connexion en dur dans le code source

---

## 📄 Licence

Ce projet est distribué sous licence **MIT**. Voir le fichier `LICENSE` pour plus de détails.

---

<div align="center">

Développé avec ❤️ · ASP.NET Core 9 · Clean Architecture · JWT

</div>
