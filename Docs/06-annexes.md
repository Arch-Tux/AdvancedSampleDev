# Annexes - AdvancedSampleDev

## 6.1 Glossaire

| Terme | Définition |
|-------|------------|
| **DDD** | Domain-Driven Design - Approche de conception centrée sur le domaine métier |
| **Value Object** | Objet immuable défini par ses valeurs (Price, Tva) |
| **Entity** | Objet avec identité unique (Product, Supplier) |
| **Repository** | Pattern d'abstraction de la persistance des données |
| **Aggregate** | Ensemble d'objets traités comme une unité (Product + Price) |
| **Invariant** | Règle métier qui doit toujours être vraie |
| **SOLID** | 5 principes de conception orientée objet |
| **KISS** | Keep It Simple, Stupid - Principe de simplicité |
| **DRY** | Don't Repeat Yourself - Éviter la duplication |
| **AAA** | Arrange, Act, Assert - Pattern de test unitaire |
| **DTO** | Data Transfer Object - Objet de transfert de données |

## 6.2 Ressources

### Documentation officielle
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [C# Language Reference](https://docs.microsoft.com/dotnet/csharp/)

### Architecture et design patterns
- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design - Eric Evans](https://www.domainlanguage.com/ddd/)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Design Patterns - Gang of Four](https://refactoring.guru/design-patterns)

### Outils
- [JetBrains Rider](https://www.jetbrains.com/rider/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Swagger/OpenAPI](https://swagger.io/)
- [Git](https://git-scm.com/)

## 6.3 Structure des fichiers

```
AdvancedSampleDev/
├── AdvancedSampleDev.sln
├── README.md
├── Docs/
│   ├── README.md
│   ├── 01-introduction.md
│   ├── 02-contexte.md
│   ├── 03-architecture.md
│   ├── 04-fonctionnement.md
│   ├── 05-procedures.md
│   └── 06-annexes.md
├── AdvancedSampleDev.Domain/
│   ├── Entities/
│   │   ├── Product.cs
│   │   ├── Price.cs
│   │   ├── Tva.cs
│   │   └── Supplier.cs
│   ├── Interfaces/
│   │   ├── IRepository.cs
│   │   ├── Product/
│   │   │   └── IProductRepository.cs
│   │   └── Supplier/
│   │       └── ISupplierRepository.cs
│   └── Exceptions/
│       └── DomainException.cs
├── AdvancedSampleDev.Application/
│   └── Services/
│       └── ProductService.cs
├── AdvancedSampleDev.Infrastructure/
│   └── (à implémenter)
├── AdvancedSampleDev.Api/
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.json
└── AdvancedSampleDev.Tests/
    └── (à implémenter)
```

## 6.4 Roadmap

### Phase 1 - Domain (En cours) ✅
- [x] Entités Product, Supplier
- [x] Value Objects Price, Tva
- [x] Interfaces IRepository<T>, IProductRepository, ISupplierRepository
- [x] Exception DomainException

### Phase 2 - Infrastructure (À venir)
- [ ] Configuration Entity Framework Core
- [ ] DbContext et migrations
- [ ] Implémentation des repositories
- [ ] Mapping entités Domain ↔ entités BDD

### Phase 3 - Application (À venir)
- [ ] ProductService complet
- [ ] SupplierService
- [ ] DTOs et mappings

### Phase 4 - API (À venir)
- [ ] ProductController
- [ ] SupplierController
- [ ] Gestion d'erreurs globale
- [ ] Authentification/Autorisation

### Phase 5 - Tests (À venir)
- [ ] Tests unitaires Domain
- [ ] Tests d'intégration Application
- [ ] Tests API

## 6.5 Changelog

### Version 1.0 (28 janvier 2026)
- Initialisation du projet
- Création de la couche Domain
- Entités Product et Supplier
- Value Objects Price et Tva
- Interfaces repositories génériques
- Documentation technique complète

---

**Version** : 1.0  
**Date de dernière mise à jour** : 28 janvier 2026  
**Auteurs** : Équipe AdvancedSampleDev  
**Statut** : En développement

---

[← Retour à la documentation principale](./README.md)
