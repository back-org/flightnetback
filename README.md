# ✈️ FlightNetBack — Plateforme Professionnelle de Gestion de Vols

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue)
![Pattern](https://img.shields.io/badge/Pattern-CQRS-green)
![Tests](https://img.shields.io/badge/Tests-xUnit-orange)
![Status](https://img.shields.io/badge/Readme-Professional-success)

## 1) Vision Produit

**FlightNetBack** est un backend enterprise conçu pour piloter les opérations d’un système aérien moderne :
- gestion des vols, réservations et passagers,
- administration des utilisateurs et rôles,
- audit des actions critiques,
- sécurité API (JWT + refresh token),
- extensibilité orientée production.

L’objectif est d’offrir une base **robuste, maintenable, scalable** et crédible en contexte professionnel (startup, DSI, compagnie aérienne, intégrateur).

---

## 2) Positionnement Technique

- **Framework**: ASP.NET Core (.NET 8)
- **Architecture**: Clean Architecture + séparation stricte des couches
- **Pattern métier**: CQRS (MediatR)
- **Persistance**: Entity Framework Core
- **Validation**: FluentValidation
- **Observabilité**: Logs + audit trail
- **Tests**: xUnit (unitaires + intégration)

---

## 3) Capacités Métier Couvertes

La plateforme couvre les domaines fonctionnels suivants :

- ✈️ **Vols** : création, modification, suivi des capacités et tarifs
- 🎫 **Réservations** : cycle de vie booking, statut, montant total, devise
- 👤 **Passagers** : identité, lien avec réservations et billets
- 🧳 **Bagages** : suivi des bagages et rattachement opérationnel
- 🧑‍✈️ **Équipage** : membres d’équipage, organisation opérationnelle
- 💳 **Paiements** : méthode, statut de transaction, référence
- 🔔 **Notifications** : diffusion d’événements métier
- 🔐 **Sécurité** : authentification, autorisation, rôles, tokens
- 📝 **Audit** : traçabilité des opérations critiques

---

## 4) Architecture Applicative

```text
API (Controllers, Middlewares)
        ↓
Application (CQRS, DTOs, Validators, Behaviors)
        ↓
Domain (Entities, règles métier, enums)
        ↓
Infrastructure (EF Core, Repositories, Auth, Audit)
```

### Principes appliqués
- responsabilités claires par couche,
- dépendances orientées vers le domaine,
- testabilité des cas d’usage,
- facilité d’évolution sans effet domino.

---

## 5) Structure du Repository

```text
src/
├── Flight.Api/                # Exposition HTTP + pipeline d'exécution
├── Flight.Application/        # Cas d'usage (CQRS), DTOs, validation
├── Flight.Domain/             # Modèle métier (Entities + Enums)
├── Flight.Domain.Core/        # Abstractions transverses du domaine
├── Flight.Infrastructure/     # Persistence, auth, audit, repository pattern
├── Flight.CrossCutting/       # Utilitaires transverses
└── Flight.Util/               # Constantes et helpers

tests/
├── Flight.UnitTests/          # Tests unitaires des handlers/commands/queries
└── Flight.IntegrationTests/   # Tests de bout en bout API
```

---

## 6) Convention de Documentation Métier des Fichiers

Pour un rendu professionnel et lisible en équipe:

- chaque fichier de code contient désormais une en-tête descriptive,
- cette en-tête explicite le **rôle métier du fichier**,
- la description relie le fichier à son sous-domaine fonctionnel.

Cette convention améliore:
- l’onboarding des développeurs,
- la compréhension fonctionnelle du code,
- la qualité perçue en audit technique.

---

## 7) Gestion des Enums Métier

Un dossier dédié a été structuré côté domaine:

- `src/Flight.Domain/Enums/`

Objectif:
- centraliser les énumérations métier,
- clarifier la sémantique des états métier,
- préparer l’extension du modèle (ex: statuts de paiement, classes tarifaires, état opérationnel).

---

## 8) Installation & Exécution

### Prérequis
- .NET SDK 8+
- Base de données compatible EF Core

### Commandes
```bash
dotnet restore
```

```bash
dotnet build
```

```bash
dotnet run --project src/Flight.Api
```

Swagger:
- `http://localhost:5000/swagger`
- ou port configuré localement dans `launchSettings` / `appsettings`

---

## 9) Qualité & Tests

Exécuter tous les tests:

```bash
dotnet test
```

Recommandations de qualité:
- garder les règles FluentValidation alignées avec les règles métier,
- maintenir la séparation Command/Query,
- versionner les migrations EF avec discipline,
- tracer les actions sensibles via audit log.

---

## 10) Valeur Professionnelle (Perception Expert)

Ce projet met en avant les marqueurs attendus d’un backend senior/lead:
- architecture maîtrisée,
- découplage des responsabilités,
- conventions de documentation métier,
- sécurité et auditabilité,
- automatisation des tests,
- clarté de maintenance long terme.

En pratique, il constitue une excellente base pour:
- démo client,
- portfolio expert,
- industrialisation en environnement réel.
