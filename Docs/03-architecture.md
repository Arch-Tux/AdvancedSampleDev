# 03 - Architecture

## 🏛️ Clean Architecture

Le projet suit les principes de la **Clean Architecture** (Onion Architecture).

```
┌─────────────────────────────────────────┐
│         API (Presentation)              │ ← Controllers, Middlewares
├─────────────────────────────────────────┤
│      Application (Use Cases)            │ ← Services, DTOs, Mappings
├─────────────────────────────────────────┤
│    Infrastructure (External)            │ ← Repositories, DB, EF Core
├─────────────────────────────────────────┤
│         Domain (Core)                   │ ← Entities, Value Objects, Interfaces
└─────────────────────────────────────────┘
```

**Règle de dépendance** : Les couches internes ne dépendent jamais des couches externes.

---

## 📂 Structure du projet

```
AdvancedSampleDev/
├── AdvancedSampleDev.sln                    # Solution principale
├── .env.example                             # Template des variables d'env
├── .gitignore                               # Fichiers à ignorer
├── README.md                                # Documentation principale
├── run-coverage.sh                          # Script de tests + couverture
│
├── AdvancedSampleDev.domain/                # 🟢 COUCHE DOMAIN
│   ├── Entities/
│   │   ├── Product.cs                       # Entité Product
│   │   ├── Supplier.cs                      # Entité Supplier
│   │   ├── Price.cs                         # Value Object Price
│   │   └── TvaType.cs                       # Enum TvaType
│   ├── Exceptions/
│   │   └── DomainException.cs               # Exceptions métier
│   └── Interfaces/
│       ├── Product/
│       │   └── IProductRepository.cs        # Interface repository Product
│       └── Supplier/
│           └── ISupplierRepository.cs       # Interface repository Supplier
│
├── AdvancedSampleDev.Application/           # 🔵 COUCHE APPLICATION
│   ├── Products/
│   │   ├── ProductService.cs                # Service métier Product
│   │   ├── Dto/
│   │   │   ├── CreateProductDto.cs
│   │   │   ├── UpdateProductDto.cs
│   │   │   └── ProductDto.cs
│   │   └── Mappings/
│   │       └── ProductMappings.cs           # Mappage Domain ↔ DTO
│   └── Suppliers/
│       ├── SupplierService.cs               # Service métier Supplier
│       ├── Dto/
│       │   ├── CreateSupplierDto.cs
│       │   ├── UpdateSupplierDto.cs
│       │   └── SupplierDto.cs
│       └── Mappings/
│           └── SupplierMappings.cs
│
├── AdvancedSampleDev.Infrastructure/        # 🟠 COUCHE INFRASTRUCTURE
│   ├── ApplicationDbContext.cs              # DbContext EF Core
│   ├── Entities/
│   │   ├── ProductEntity.cs                 # Modèle DB Product
│   │   └── SupplierEntity.cs                # Modèle DB Supplier
│   ├── Repositories/
│   │   ├── ProductRepository.cs             # Implémentation repository
│   │   └── SupplierRepository.cs
│   ├── Seed/
│   │   └── DatabaseSeeder.cs                # Seed données de test
│   └── Extensions/
│       └── ServiceCollectionExtensions.cs   # Extensions DI
│
├── AdvancedSampleDev.Api/                   # 🔴 COUCHE API
│   ├── Program.cs                           # Point d'entrée
│   ├── appsettings.json                     # Configuration (sans secrets)
│   ├── Features/
│   │   ├── Auth/
│   │   │   ├── AuthController.cs            # Contrôleur authentification
│   │   │   └── AuthDtos.cs                  # DTOs login
│   │   ├── Product/
│   │   │   └── ProductsController.cs        # Contrôleur products
│   │   └── Supplier/
│   │       └── SuppliersController.cs       # Contrôleur suppliers
│   ├── Middlewares/
│   │   ├── JwtAuthenticationExtensions.cs   # Config JWT
│   │   └── MiddlewareExtensions.cs          # Pipeline middlewares
│   └── Services/
│       └── TokenService.cs                  # Génération tokens JWT
│
├── AdvancedSampleDev.Cli/                   # 🛠️ OUTIL CLI
│   ├── Program.cs                           # CLI pour DB management
│   └── README.md
│
├── AdvancedSampleDev.Tests/                 # 🧪 TESTS
│   ├── Domain/                              # Tests unitaires domain
│   │   ├── PriceTests.cs
│   │   ├── ProductTests.cs
│   │   ├── SupplierTests.cs
│   │   └── TvaTypeTests.cs
│   ├── Application/                         # Tests unitaires application
│   │   ├── ProductServiceTests.cs
│   │   └── SupplierServiceTests.cs
│   └── Integration/                         # Tests d'intégration
│       ├── IntegrationTestBase.cs
│       ├── ProductRepositoryIntegrationTests.cs
│       ├── SupplierRepositoryIntegrationTests.cs
│       └── AuthenticationTests.cs
│
└── Docs/                                    # 📚 DOCUMENTATION
    ├── 01-introduction.md
    ├── 02-contexte.md
    ├── 03-architecture.md (ce fichier)
    ├── 04-fonctionnement.md
    ├── 05-procedures.md
    └── 06-annexes.md
```

---

## 🟢 Couche Domain (Cœur métier)

### Responsabilités
- Définir les **entités** et **value objects**
- Gérer les **règles métier** et invariants
- Définir les **interfaces** des repositories
- **Aucune dépendance** externe (pas de EF Core, pas d'ASP.NET)

### Fichiers clés

**Entities/**
- `Product.cs` - Entité produit avec méthodes métier
- `Supplier.cs` - Entité fournisseur
- `Price.cs` - Value Object immuable pour les prix
- `TvaType.cs` - Enum pour les types de TVA

**Interfaces/**
- `IProductRepository.cs` - Contrat du repository (implémenté dans Infrastructure)
- `ISupplierRepository.cs` - Contrat du repository

---

## 🔵 Couche Application (Cas d'usage)

### Responsabilités
- Orchestrer la **logique applicative**
- Implémenter les **use cases**
- Gérer les **DTOs** (Data Transfer Objects)
- Mapper entre Domain et DTOs

### Fichiers clés

**Services/**
- `ProductService.cs` - Logique applicative produits (CRUD)
- `SupplierService.cs` - Logique applicative fournisseurs

**DTOs/**
- `CreateProductDto.cs` - Données pour créer un produit
- `UpdateProductDto.cs` - Données pour mettre à jour
- `ProductDto.cs` - Données pour la réponse

**Mappings/**
- `ProductMappings.cs` - Extensions de mapping Domain ↔ DTO

---

## 🟠 Couche Infrastructure (Techniques)

### Responsabilités
- Implémenter les **repositories**
- Gérer la **persistence** (EF Core + SQLite)
- Configurer le **DbContext**
- Seed des données de test

### Fichiers clés

**Repositories/**
- `ProductRepository.cs` - Implémentation CRUD avec EF Core
- `SupplierRepository.cs`

**Entities/**
- `ProductEntity.cs` - Modèle de table DB (différent du Domain !)
- `SupplierEntity.cs`

**DbContext**
- `ApplicationDbContext.cs` - Configuration EF Core

**Seed/**
- `DatabaseSeeder.cs` - Faker pour générer des données de test

---

## 🔴 Couche API (Présentation)

### Responsabilités
- Exposer les **endpoints HTTP**
- Gérer l'**authentification JWT**
- Valider les **entrées**
- Retourner des **réponses HTTP** (JSON)

### Fichiers clés

**Controllers/**
- `AuthController.cs` - Login et génération de tokens
- `ProductsController.cs` - CRUD produits
- `SuppliersController.cs` - CRUD fournisseurs

**Middlewares/**
- `JwtAuthenticationExtensions.cs` - Configuration JWT
- `MiddlewareExtensions.cs` - Pipeline de middlewares

**Services/**
- `TokenService.cs` - Génération et validation des JWT

---

## 🧪 Couche Tests

### Organisation

**Domain/** - Tests unitaires
- Vérifie la logique métier des entités
- Pas de dépendances externes
- Très rapides (millisecondes)

**Application/** - Tests unitaires
- Vérifie les services applicatifs
- Mock des repositories avec Moq
- Isolation totale

**Integration/** - Tests d'intégration
- Vérifie les repositories avec une vraie DB (SQLite in-memory)
- Teste le mapping EF Core
- Plus lents mais réalistes

---

## 🔄 Flux de dépendances

```
API → Application → Domain ← Infrastructure
```

**Injection de dépendances (DI) :**

```csharp
// Program.cs
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
```

**Inversion de contrôle :**
- Domain définit `IProductRepository` (interface)
- Infrastructure implémente `ProductRepository` (classe concrète)
- Application dépend de l'interface, pas de l'implémentation

---

## 📊 Diagramme de classes simplifié

```
┌─────────────────┐
│    Product      │
├─────────────────┤
│ + Id: Guid      │
│ + Name: string  │
│ + Price: Price  │───┐
│ + IsActive:bool │   │
├─────────────────┤   │   ┌──────────────┐
│ + Activate()    │   └──→│    Price     │
│ + Deactivate()  │       ├──────────────┤
│ + ChangeName()  │       │+ AmountHT    │
│ + ChangePrice() │       │+ TvaType     │
└─────────────────┘       ├──────────────┤
                          │+ GetTTC()    │
                          └──────────────┘

┌─────────────────┐
│   Supplier      │
├─────────────────┤
│ + Id: Guid      │
│ + Name: string  │
├─────────────────┤
│ + ChangeName()  │
└─────────────────┘
```

---

## 🎯 Principes SOLID appliqués

### S - Single Responsibility
- Chaque classe a **une seule responsabilité**
- `ProductService` : logique applicative
- `ProductRepository` : persistence

### O - Open/Closed
- Ouvert à l'extension, fermé à la modification
- Ajout de nouvelles entités sans modifier l'existant

### L - Liskov Substitution
- Les implémentations respectent les interfaces
- `ProductRepository` remplaçable par un mock dans les tests

### I - Interface Segregation
- Interfaces spécifiques et ciblées
- `IProductRepository` : uniquement CRUD produits

### D - Dependency Inversion
- Dépendance sur les abstractions (interfaces)
- Pas de dépendance directe sur les implémentations

---

**Prochaine section** : [04 - Fonctionnement](04-fonctionnement.md)
