# Documentation Technique - AdvancedSampleDev

## 1. Introduction

### 1.1 Objet du document
Ce document présente la documentation technique du projet **AdvancedSampleDev**, une application de gestion de catalogue de produits développée en C# .NET.

### 1.2 Public visé
- Développeurs backend C# .NET
- Architectes logiciels
- Équipes de maintenance et support technique

### 1.3 Périmètre
Cette documentation couvre :
- L'architecture du système
- Les couches applicatives et leurs responsabilités
- Les modèles de domaine et leurs règles métier
- Les interfaces et contrats
- Les procédures de développement et déploiement

---

## 2. Contexte

### 2.1 Vue d'ensemble du projet
**AdvancedSampleDev** est une application backend basée sur une architecture en couches (Clean Architecture / DDD) pour la gestion d'un catalogue de produits avec gestion des prix, de la TVA et des fournisseurs.

### 2.2 Technologies utilisées
- **Framework** : .NET 8.0 / ASP.NET Core
- **Langage** : C# 12
- **Architecture** : Domain-Driven Design (DDD) + Clean Architecture
- **API** : REST API avec OpenAPI/Swagger

### 2.3 Objectifs métier
- Gérer un catalogue de produits avec prix et TVA
- Assurer la validité des données métier via encapsulation et invariants
- Gérer les relations produits-fournisseurs
- Respecter les principes SOLID, KISS, DRY, DDD, Clean architecture et Clean Code

### 2.4 Principes de conception appliqués
- **SOLID** : Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **KISS** : Keep It Simple, Stupid
- **DRY** : Don't Repeat Yourself
- **DDD** : Modélisation centrée sur le domaine métier
- **Clean Architecture** : Séparation claire des couches (Domain, Application, Infrastructure, API)
- **Clean Code** : Lisibilité, maintenabilité, tests unitaires
- **Encapsulation** : Validation et invariants au niveau du domaine
- **Immutabilité** : Value Objects immuables (Price, Tva)

---

## 3. Architecture

### 3.1 Vue globale

Le projet suit une architecture en couches séparant clairement les responsabilités :

```
┌─────────────────────────────────────────┐
│     AdvancedSampleDev.Api (Présentation)│
│     - Controllers                       │
│     - Configuration API                 │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│  AdvancedSampleDev.Application          │
│  - Services métier                      │
│  - Orchestration                        │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│  AdvancedSampleDev.Domain               │
│  - Entités                              │
│  - Value Objects                        │
│  - Interfaces (Repositories)            │
│  - Exceptions métier                    │
└──────────────────┬──────────────────────┘
                   │
┌──────────────────▼──────────────────────┐
│  AdvancedSampleDev.Infrastructure       │
│  - Repositories (implémentations)       │
│  - DbContext                            │
│  - Entités de persistance               │
└─────────────────────────────────────────┘
```

### 3.2 Description des couches

#### 3.2.1 Domain (Cœur métier)
**Responsabilité** : Contient la logique métier pure, indépendante de toute infrastructure.

**Composants** :
- **Entités** : `Product`, `Supplier`
- **Value Objects** : `Price`, `Tva`
- **Interfaces** : `IRepository<T>`, `IProductRepository`, `ISupplierRepository`
- **Exceptions** : `DomainException`

**Règles** :
- Aucune dépendance externe (pas de référence à d'autres couches)
- Encapsulation stricte avec propriétés en `private set`
- Validation dans les constructeurs et méthodes métier
- Immutabilité pour les Value Objects

#### 3.2.2 Application (Logique applicative)
**Responsabilité** : Orchestre les cas d'usage métier en utilisant le domaine et les repositories.

**Composants** :
- **Services** : `ProductService` (à implémenter)
- DTOs (Data Transfer Objects) - à venir
- Mappings Domain ↔ DTOs - à venir

**Règles** :
- Dépend uniquement du Domain (via les interfaces)
- Ne contient pas de logique métier (délégation au Domain)
- Gère les transactions et l'orchestration

#### 3.2.3 Infrastructure (Persistance et services externes)
**Responsabilité** : Implémente les interfaces du domaine (repositories, services externes).

**Composants** (à implémenter) :
- **Repositories** : Implémentations concrètes de `IProductRepository`, `ISupplierRepository`
- **DbContext** : Configuration Entity Framework Core
- **Entités de persistance** : Mapping ORM vers la base de données

**Règles** :
- Implémente les interfaces définies dans le Domain
- Gère le mapping entre entités Domain et entités BDD
- Configuration EF Core (migrations, relations, etc.)

#### 3.2.4 API (Présentation)
**Responsabilité** : Expose les fonctionnalités via une API REST.

**Composants** :
- **Controllers** : `WeatherForecastController` (exemple), `ProductController` (à venir)
- **Configuration** : OpenAPI/Swagger, CORS, authentification
- **Middlewares** : Gestion d'erreurs, logging

**Règles** :
- Dépend de Application et Infrastructure
- Gère uniquement l'HTTP (pas de logique métier)
- Injection de dépendances via `Program.cs`

#### 3.2.5 Tests
**Responsabilité** : Tests unitaires et d'intégration.

**Composants** (à implémenter) :
- Tests unitaires du Domain
- Tests d'intégration des Services
- Tests API

**Principes** :
- Pattern AAA (Arrange, Act, Assert)
- Mock des dépendances externes

### 3.3 Modèle de domaine

#### 3.3.1 Entités

##### Product
```csharp
public class Product
{
  public Guid Id { get; private set; }
  public Price Price { get; private set; }
  private bool IsActive { get; set; }
  public ICollection<Supplier> Suppliers { get; private set; }
  
  // Méthodes métier : ChangePrice()
}
```

**Invariants** :
- Un produit doit toujours avoir un prix valide (non null)
- Seuls les produits actifs peuvent changer de prix
- Id généré automatiquement (GUID)

##### Supplier
```csharp
public class Supplier
{
  public Guid Id { get; }
  public string Name { get; }
}
```

**Invariants** :
- Le nom ne peut pas être vide ou null
- Id généré automatiquement (GUID)

#### 3.3.2 Value Objects

##### Price
```csharp
public class Price
{
  private decimal AmountHt { get; }
  private Tva Tva { get; }
  private decimal AmountTtc => Tva.CalculateTtcFromHt(AmountHt);
}
```

**Caractéristiques** :
- Immuable (pas de setters)
- Equality basée sur la valeur (override Equals/GetHashCode)
- Validation : prix HT > 0

##### Tva
```csharp
public class Tva
{
  public decimal Rate { get; }
  
  // Singletons pour les 3 taux français
  public static Tva Reduced => 0.055m;      // 5,5%
  public static Tva Intermediate => 0.10m;   // 10%
  public static Tva Standard => 0.20m;       // 20%
}
```

**Caractéristiques** :
- Pattern Singleton pour les 3 taux de TVA français
- Méthodes de calcul : `CalculateTtcFromHt()`, `CalculateHtFromTtc()`
- Immuable

### 3.4 Interfaces et contrats

#### IRepository<T>
Interface générique de base pour tous les repositories :

```csharp
public interface IRepository<T> where T : class
{
  Task<T?> GetByIdAsync(Guid id);
  Task<IEnumerable<T>> GetAllAsync();
  Task AddAsync(T entity);
  Task UpdateAsync(T entity);
  Task DeleteAsync(Guid id);
  Task<bool> ExistsAsync(Guid id);
}
```

#### IProductRepository
Hérite de `IRepository<Product>` et ajoute des méthodes spécifiques :

```csharp
public interface IProductRepository : IRepository<Product>
{
  Task<IEnumerable<Product>> GetBySupplierIdAsync(Guid supplierId);
}
```

#### ISupplierRepository
Hérite de `IRepository<Supplier>` sans méthodes spécifiques pour le moment.

---

## 4. Fonctionnement

### 4.1 Flux de données

#### Création d'un produit
```
Client HTTP
    │
    ▼
[ProductController] (API)
    │
    ▼
[ProductService] (Application)
    │
    ├─► Crée Price (Domain)
    ├─► Crée Product (Domain)
    │
    ▼
[IProductRepository] (Interface Domain)
    │
    ▼
[ProductRepository] (Infrastructure)
    │
    ▼
Base de données
```

#### Changement de prix
```
Client HTTP
    │
    ▼
[ProductController]
    │
    ▼
[ProductService]
    │
    ├─► GetByIdAsync() → Product
    ├─► Product.ChangePrice(newPrice) → Validation métier
    │
    ▼
[ProductRepository.UpdateAsync()]
    │
    ▼
Base de données
```

### 4.2 Règles métier

#### Gestion des prix
1. Un prix doit toujours être positif (> 0)
2. Un prix est composé d'un montant HT et d'une TVA
3. Le calcul TTC est automatique via la TVA
4. Un produit ne peut changer de prix que s'il est actif

#### Gestion de la TVA
1. Trois taux uniquement : 5.5%, 10%, 20% (France)
2. Taux hardcodés et immuables (Singleton)
3. Calculs : HT → TTC et TTC → HT

#### Gestion des fournisseurs
1. Un fournisseur doit avoir un nom non vide
2. Un produit peut avoir plusieurs fournisseurs
3. L'association produit-fournisseur est gérée par la couche Application/Infrastructure

### 4.3 Gestion des erreurs

#### DomainException
Exception personnalisée pour les violations de règles métier :
- Prix invalide (≤ 0 ou null)
- TVA null
- Nom de fournisseur vide
- Tentative de changement de prix sur produit inactif

**Principe** : Fail-fast - les erreurs sont détectées au plus tôt (constructeurs, méthodes métier)

---

## 5. Procédures

### 5.1 Configuration de l'environnement de développement

#### Prérequis
- .NET SDK 8.0 ou supérieur
- IDE : JetBrains Rider, Visual Studio 2022, ou VS Code
- Git

#### Installation
```bash
# Cloner le repository
git clone https://github.com/Arch-Tux/AdvancedSampleDev.git
cd AdvancedSampleDev

# Restaurer les dépendances
dotnet restore

# Build de la solution
dotnet build
```

### 5.2 Exécution du projet

#### Lancer l'API
```bash
cd AdvancedSampleDev.Api
dotnet run
```

L'API sera accessible sur : `https://localhost:5001` (ou port configuré)

#### Swagger UI
Accessible en mode Development : `https://localhost:5001/openapi`

### 5.3 Tests

#### Exécuter tous les tests
```bash
dotnet test
```

#### Exécuter les tests d'un projet spécifique
```bash
dotnet test AdvancedSampleDev.Tests/AdvancedSampleDev.Tests.csproj
```

#### Pattern AAA (Arrange, Act, Assert)
```csharp
[Fact]
public void ChangePrice_ShouldUpdatePrice_WhenProductIsActive()
{
    // Arrange
    var tva = Tva.Standard;
    var price = new Price(100m, tva);
    var product = new Product(price);
    var newPrice = new Price(150m, tva);
    
    // Act
    product.ChangePrice(newPrice);
    
    // Assert
    Assert.Equal(newPrice, product.Price);
}
```

### 5.4 Contribution

#### Workflow Git
```bash
# Créer une branche feature
git checkout -b feat-nom-feature

# Commiter les changements
git add .
git commit -m "feat: description de la fonctionnalité"

# Pousser la branche
git push origin feat-nom-feature

# Créer une Pull Request sur GitHub
```

#### Conventions de nommage
- **Branches** : `feat-`, `fix-`, `docs-`, `refactor-`
- **Commits** : Convention Conventional Commits
  - `feat:` Nouvelle fonctionnalité
  - `fix:` Correction de bug
  - `docs:` Documentation
  - `refactor:` Refactoring sans changement fonctionnel
  - `test:` Ajout/modification de tests

### 5.5 Déploiement

#### Build en production
```bash
dotnet publish -c Release -o ./publish
```

#### Variables d'environnement
À configurer selon l'environnement :
- `ASPNETCORE_ENVIRONMENT` : `Development`, `Staging`, `Production`
- Chaînes de connexion BDD (à venir)
- Secrets API (à venir)

---

## 6. Annexes

### 6.1 Glossaire

| Terme | Définition |
|-------|------------|
| **DDD** | Domain-Driven Design - Approche de conception centrée sur le domaine métier |
| **Value Object** | Objet immuable défini par ses valeurs (Price, Tva) |
| **Entity** | Objet avec identité unique (Product, Supplier) |
| **Repository** | Pattern d'abstraction de la persistance des données |
| **Aggregate** | Ensemble d'objets traités comme une unité (Product + Price) |
| **Invariant** | Règle métier qui doit toujours être vraie |
| **SOLID** | 5 principes de conception orientée objet |

### 6.2 Ressources

#### Documentation officielle
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)

#### Architecture et design patterns
- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design - Eric Evans](https://www.domainlanguage.com/ddd/)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)

#### Outils
- [JetBrains Rider](https://www.jetbrains.com/rider/)
- [Swagger/OpenAPI](https://swagger.io/)

### 6.3 Structure des fichiers

```
AdvancedSampleDev/
├── AdvancedSampleDev.sln
├── README.md
├── Docs/
│   └── documentation-technique.md
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

### 6.4 Diagrammes

#### Diagramme de classes (Domain)

```
┌─────────────────┐
│    Product      │
├─────────────────┤
│ + Id: Guid      │
│ + Price: Price  │◄──────┐
│ - IsActive: bool│       │
│ + Suppliers: [] │       │
├─────────────────┤       │
│ + ChangePrice() │       │
└─────────────────┘       │
                          │
                    ┌─────┴──────┐
                    │   Price    │
                    ├────────────┤
                    │ - AmountHt │
                    │ - Tva      │◄──────┐
                    │ + AmountTtc│       │
                    └────────────┘       │
                                         │
                                   ┌─────┴────────┐
                                   │    Tva       │
                                   ├──────────────┤
                                   │ + Rate       │
                                   ├──────────────┤
                                   │ «static»     │
                                   │ Reduced      │
                                   │ Intermediate │
                                   │ Standard     │
                                   └──────────────┘

┌─────────────────┐
│   Supplier      │
├─────────────────┤
│ + Id: Guid      │
│ + Name: string  │
└─────────────────┘
```

### 6.5 Checklist de développement

#### Avant de créer une Pull Request
- [ ] Le code compile sans erreur ni warning
- [ ] Les tests unitaires passent
- [ ] Le code respecte les principes SOLID
- [ ] L'encapsulation est respectée (pas de setters publics)
- [ ] Les exceptions métier utilisent `DomainException`
- [ ] La documentation est à jour
- [ ] Les commits suivent les conventions

#### Revue de code
- [ ] Les règles métier sont dans le Domain
- [ ] Pas de logique métier dans l'API ou l'Infrastructure
- [ ] Les Value Objects sont immuables
- [ ] Les repositories utilisent les interfaces du Domain
- [ ] Pattern AAA respecté dans les tests

### 6.6 Roadmap

#### Phase 1 - Domain (En cours) ✅
- [x] Entités Product, Supplier
- [x] Value Objects Price, Tva
- [x] Interfaces IRepository<T>, IProductRepository, ISupplierRepository
- [x] Exception DomainException

#### Phase 2 - Infrastructure (À venir)
- [ ] Configuration Entity Framework Core
- [ ] DbContext et migrations
- [ ] Implémentation des repositories
- [ ] Mapping entités Domain ↔ entités BDD

#### Phase 3 - Application (À venir)
- [ ] ProductService complet
- [ ] SupplierService
- [ ] DTOs et mappings

#### Phase 4 - API (À venir)
- [ ] ProductController
- [ ] SupplierController
- [ ] Gestion d'erreurs globale
- [ ] Authentification/Autorisation

#### Phase 5 - Tests (À venir)
- [ ] Tests unitaires Domain
- [ ] Tests d'intégration Application
- [ ] Tests API

---

**Version** : 1.0  
**Date de dernière mise à jour** : 28 janvier 2026  
**Auteurs** : Équipe AdvancedSampleDev  
**Statut** : En développement
