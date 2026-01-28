# Architecture - AdvancedSampleDev

## 3.1 Vue globale

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

## 3.2 Description des couches

### 3.2.1 Domain (Cœur métier)
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

### 3.2.2 Application (Logique applicative)
**Responsabilité** : Orchestre les cas d'usage métier en utilisant le domaine et les repositories.

**Composants** :
- **Services** : `ProductService` (à implémenter)
- DTOs (Data Transfer Objects) - à venir
- Mappings Domain ↔ DTOs - à venir

**Règles** :
- Dépend uniquement du Domain (via les interfaces)
- Ne contient pas de logique métier (délégation au Domain)
- Gère les transactions et l'orchestration

### 3.2.3 Infrastructure (Persistance et services externes)
**Responsabilité** : Implémente les interfaces du domaine (repositories, services externes).

**Composants** (à implémenter) :
- **Repositories** : Implémentations concrètes de `IProductRepository`, `ISupplierRepository`
- **DbContext** : Configuration Entity Framework Core
- **Entités de persistance** : Mapping ORM vers la base de données

**Règles** :
- Implémente les interfaces définies dans le Domain
- Gère le mapping entre entités Domain et entités BDD
- Configuration EF Core (migrations, relations, etc.)

### 3.2.4 API (Présentation)
**Responsabilité** : Expose les fonctionnalités via une API REST.

**Composants** :
- **Controllers** : `WeatherForecastController` (exemple), `ProductController` (à venir)
- **Configuration** : OpenAPI/Swagger, CORS, authentification
- **Middlewares** : Gestion d'erreurs, logging

**Règles** :
- Dépend de Application et Infrastructure
- Gère uniquement l'HTTP (pas de logique métier)
- Injection de dépendances via `Program.cs`

### 3.2.5 Tests
**Responsabilité** : Tests unitaires et d'intégration.

**Composants** (à implémenter) :
- Tests unitaires du Domain
- Tests d'intégration des Services
- Tests API

**Principes** :
- Pattern AAA (Arrange, Act, Assert)
- Mock des dépendances externes

## 3.3 Modèle de domaine

### 3.3.1 Entités

#### Product
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

#### Supplier
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

### 3.3.2 Value Objects

#### Price
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

#### Tva
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

## 3.4 Interfaces et contrats

### IRepository<T>
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

### IProductRepository
Hérite de `IRepository<Product>` et ajoute des méthodes spécifiques :

```csharp
public interface IProductRepository : IRepository<Product>
{
  Task<IEnumerable<Product>> GetBySupplierIdAsync(Guid supplierId);
}
```

### ISupplierRepository
Hérite de `IRepository<Supplier>` sans méthodes spécifiques pour le moment.

## 3.5 Diagramme de classes (Domain)

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

---

[← Retour à la documentation principale](./README.md)
