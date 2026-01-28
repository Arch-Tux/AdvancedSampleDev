# Documentation Technique - AdvancedSampleDev

> **Application de gestion de catalogue de produits** développée en C# .NET avec une architecture Clean Architecture / DDD

---

## 📚 Table des matières

### [1. Introduction](./01-introduction.md)
- Objet du document
- Public visé
- Périmètre de la documentation

### [2. Contexte](./02-contexte.md)
- Vue d'ensemble du projet
- Technologies utilisées (.NET 8, C# 12, ASP.NET Core)
- Objectifs métier
- Principes de conception (SOLID, KISS, DRY, DDD, Clean Architecture, Clean Code)

### [3. Architecture](./03-architecture.md)
- Vue globale de l'architecture en couches
- Description détaillée des couches :
  - **Domain** : Entités, Value Objects, Interfaces, Exceptions
  - **Application** : Services, Orchestration
  - **Infrastructure** : Repositories, DbContext, Persistance
  - **API** : Controllers, Configuration
  - **Tests** : Tests unitaires et d'intégration
- Modèle de domaine (Product, Price, Tva, Supplier)
- Interfaces et contrats (IRepository<T>, IProductRepository, ISupplierRepository)
- Diagrammes de classes

### [4. Fonctionnement](./04-fonctionnement.md)
- Flux de données (création de produit, changement de prix)
- Règles métier (prix, TVA, fournisseurs)
- Gestion des erreurs (DomainException, Fail-fast)

### [5. Procédures](./05-procedures.md)
- Configuration de l'environnement de développement
- Installation et exécution du projet
- Tests (Pattern AAA)
- Workflow Git et conventions de nommage
- Déploiement
- Checklists de développement

### [6. Annexes](./06-annexes.md)
- Glossaire (DDD, Value Object, Entity, Repository, SOLID, etc.)
- Ressources et documentation officielle
- Structure des fichiers
- Roadmap du projet
- Changelog

---

## 🚀 Démarrage rapide

```bash
# Cloner le repository
git clone https://github.com/Arch-Tux/AdvancedSampleDev.git
cd AdvancedSampleDev

# Restaurer et build
dotnet restore
dotnet build

# Lancer l'API
cd AdvancedSampleDev.Api
dotnet run
```

**Swagger** : `https://localhost:5001/openapi`

---

## 🏗️ Architecture en bref

```
┌─────────────────────────────────────────┐
│     API (Présentation)                  │
│     ↓                                   │
│     Application (Orchestration)         │
│     ↓                                   │
│     Domain (Logique métier)             │
│     ↓                                   │
│     Infrastructure (Persistance)        │
└─────────────────────────────────────────┘
```

**Principes** : SOLID • KISS • DRY • DDD • Clean Architecture

---

## 📊 État du projet

### ✅ Phase 1 - Domain (Complété)
- Entités : Product, Supplier
- Value Objects : Price, Tva
- Interfaces : IRepository<T>, IProductRepository, ISupplierRepository
- Exceptions : DomainException

### 🚧 Phases suivantes
- **Phase 2** : Infrastructure (EF Core, Repositories)
- **Phase 3** : Application (Services, DTOs)
- **Phase 4** : API (Controllers, Middlewares)
- **Phase 5** : Tests (Unitaires, Intégration)

Voir la [roadmap complète](./06-annexes.md#64-roadmap)

---

## 👥 Contribution

Ce projet suit les conventions **Conventional Commits** :
- `feat:` Nouvelle fonctionnalité
- `fix:` Correction de bug
- `docs:` Documentation
- `refactor:` Refactoring
- `test:` Tests

Voir le [guide de contribution](./05-procedures.md#54-contribution)

---

## 📖 Documentation détaillée

Pour plus de détails, consultez les documents spécifiques :

| Document | Description |
|----------|-------------|
| [Introduction](./01-introduction.md) | Présentation générale |
| [Contexte](./02-contexte.md) | Technologies et principes |
| [Architecture](./03-architecture.md) | Structure et modèle de domaine |
| [Fonctionnement](./04-fonctionnement.md) | Flux et règles métier |
| [Procédures](./05-procedures.md) | Développement et déploiement |
| [Annexes](./06-annexes.md) | Glossaire, ressources, roadmap |

---

**Version** : 1.0  
**Dernière mise à jour** : 28 janvier 2026  
**Statut** : 🚧 En développement
