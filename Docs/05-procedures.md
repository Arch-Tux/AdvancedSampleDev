# 05 - Procédures

## 🚀 Installation et démarrage

### Prérequis

- **.NET 10.0 SDK** - [Télécharger](https://dotnet.microsoft.com/download)
- **Git** - Pour cloner le repository
- **Un éditeur** - VS Code, Rider, Visual Studio

Vérifier l'installation :
```bash
dotnet --version  # Doit afficher 10.x.x
```

### 1. Cloner le projet

```bash
git clone https://github.com/Arch-Tux/AdvancedSampleDev.git
cd AdvancedSampleDev
```

### 2. Configurer les variables d'environnement

```bash
# Créer le fichier .env
cp .env.example .env

# Le fichier .env doit contenir :
# JWT_SECRET_KEY=VotreCleSuperSecreteQuiDoitEtreTresLongue!MinimumMinimum32Caracteres
```

**⚠️ Important :** Le fichier `.env` est dans `.gitignore` et ne sera jamais commité.

### 3. Initialiser la base de données

```bash
# Créer les tables et seed avec des données de test
dotnet run --project AdvancedSampleDev.Cli db-reset
```

**Résultat :**
- Création du fichier `advancedsampledev.db` à la racine
- 20 suppliers générés
- 100 produits générés

### 4. Lancer l'API

```bash
dotnet run --project AdvancedSampleDev.Api
```

**Accès :**
- API : http://localhost:5155
- Scalar UI : http://localhost:5155/scalar/v1

---

## 🔐 Authentification

### Générer un token JWT

**Dans un terminal :**

```bash
curl -X POST http://localhost:5155/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'
```

**Réponse :**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-02-13T16:30:00Z"
}
```

**Copie le token** (commence par `eyJ...`)

### Utiliser le token

**Dans Scalar UI :**
1. Ouvre http://localhost:5155/scalar/v1
2. Va sur un endpoint (ex: GET /api/products)
3. Dans "Headers", ajoute :
   - Key : `Authorization`
   - Value : `Bearer eyJhbGci...` (avec un espace après "Bearer")

**Avec cURL :**
```bash
curl -X GET http://localhost:5155/api/products \
  -H "Authorization: Bearer <ton-token-ici>"
```

---

## 🧪 Exécuter les tests

### Tous les tests

```bash
dotnet test
```

**Résultat attendu :**
```
Total tests: 85
     Passed: 85
     Failed: 0
```

### Tests unitaires uniquement

```bash
dotnet test --filter "FullyQualifiedName~Domain|FullyQualifiedName~Application"
```

### Tests d'intégration uniquement

```bash
dotnet test --filter "FullyQualifiedName~Integration"
```

### Avec couverture de code

```bash
./run-coverage.sh
```

**Ouvre le rapport :** `coveragereport/index.html`

---

## 🗄️ Gestion de la base de données

### Commandes CLI disponibles

```bash
# Créer les tables (sans seed)
dotnet run --project AdvancedSampleDev.Cli db-create

# Supprimer la base de données
dotnet run --project AdvancedSampleDev.Cli db-drop

# Reset complet (drop + create + seed)
dotnet run --project AdvancedSampleDev.Cli db-reset

# Seed uniquement (avec données existantes)
dotnet run --project AdvancedSampleDev.Cli seed

# Aide
dotnet run --project AdvancedSampleDev.Cli help
```

### Localisation de la base de données

**Fichier :** `advancedsampledev.db` (à la racine du projet)

**Visualiser avec un outil :**
```bash
# Avec DB Browser for SQLite
# Télécharger : https://sqlitebrowser.org/

# Ou avec sqlite3 CLI
sqlite3 advancedsampledev.db
sqlite> .tables
sqlite> SELECT * FROM Products LIMIT 5;
```

---

## 📡 Tester l'API

### Avec Scalar UI (recommandé)

1. **Ouvrir** http://localhost:5155/scalar/v1
2. **Se connecter** via POST /api/auth/login
3. **Copier le token**
4. **L'utiliser** dans le header `Authorization: Bearer <token>`
5. **Tester** les endpoints

### Avec cURL

#### Lister les produits

```bash
curl -X GET http://localhost:5155/api/products \
  -H "Authorization: Bearer <token>"
```

#### Créer un produit

```bash
curl -X POST http://localhost:5155/api/products \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Nouveau Produit",
    "priceHT": 99.99,
    "tvaType": "Standard"
  }'
```

#### Mettre à jour un produit

```bash
curl -X PUT http://localhost:5155/api/products/<id> \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Produit Modifié",
    "priceHT": 149.99,
    "tvaType": "Standard"
  }'
```

#### Supprimer un produit

```bash
curl -X DELETE http://localhost:5155/api/products/<id> \
  -H "Authorization: Bearer <token>"
```

### Avec Postman

1. **Créer une collection** "AdvancedSampleDev"
2. **Variables de collection** :
   - `base_url` : `http://localhost:5155`
   - `token` : (généré via /api/auth/login)
3. **Headers globaux** :
   - `Authorization` : `Bearer {{token}}`
   - `Content-Type` : `application/json`

---

## 🔧 Développement

### Ajouter une nouvelle entité

#### 1. Créer l'entité Domain

```csharp
// AdvancedSampleDev.domain/Entities/Category.cs
public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty");
        
        Id = Guid.NewGuid();
        Name = name;
    }
}
```

#### 2. Créer l'interface repository

```csharp
// AdvancedSampleDev.domain/Interfaces/Category/ICategoryRepository.cs
public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Guid id);
}
```

#### 3. Créer l'entité Infrastructure

```csharp
// AdvancedSampleDev.Infrastructure/Entities/CategoryEntity.cs
public class CategoryEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
```

#### 4. Configurer dans DbContext

```csharp
// ApplicationDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<CategoryEntity>(entity =>
    {
        entity.ToTable("Categories");
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
    });
}
```

#### 5. Implémenter le repository

```csharp
// AdvancedSampleDev.Infrastructure/Repositories/CategoryRepository.cs
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    
    // Implémenter les méthodes...
}
```

#### 6. Créer le service

```csharp
// AdvancedSampleDev.Application/Categories/CategoryService.cs
public class CategoryService
{
    // Logique applicative...
}
```

#### 7. Créer le contrôleur

```csharp
// AdvancedSampleDev.Api/Features/Category/CategoriesController.cs
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    // Endpoints CRUD...
}
```

#### 8. Enregistrer dans Program.cs

```csharp
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<CategoryService>();
```

---

## 🐛 Dépannage

### L'API ne démarre pas

**Erreur :** `JWT_SECRET_KEY non définie`

**Solution :**
```bash
# Vérifier que le fichier .env existe
ls -la .env

# Vérifier son contenu
cat .env

# S'il n'existe pas, le créer
cp .env.example .env
```

### Erreur de base de données

**Erreur :** `no such table: Products`

**Solution :**
```bash
# Recréer la base
dotnet run --project AdvancedSampleDev.Cli db-reset
```

### Port déjà utilisé

**Erreur :** `Address already in use: 5155`

**Solution :**
```bash
# macOS/Linux
lsof -ti:5155 | xargs kill -9

# Windows
netstat -ano | findstr :5155
taskkill /PID <PID> /F
```

### Token JWT expiré

**Erreur :** `401 Unauthorized`

**Solution :**
```bash
# Générer un nouveau token
curl -X POST http://localhost:5155/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'
```

---

## 📊 CI/CD

### Workflow GitHub Actions

**Tests automatiques :**
- Déclenchés sur push/PR vers `dev` ou `main`
- 85 tests exécutés
- Rapport de couverture généré
- Variable d'env `JWT_SECRET_KEY` injectée

**SonarQube Cloud :**
- Analyse de qualité de code
- Quality Gate vérifié
- Résultats visibles sur SonarCloud

### Lancer localement

```bash
# Comme la CI
dotnet restore
dotnet build --configuration Release
dotnet test

# Vérifier SonarQube localement (nécessite SonarScanner)
dotnet sonarscanner begin /k:"projet" /d:sonar.token="<token>"
dotnet build
dotnet sonarscanner end /d:sonar.token="<token>"
```

---

## 🎓 Bonnes pratiques

### Commits

```bash
# Format recommandé
git commit -m "feat: ajout de l'entité Category"
git commit -m "fix: correction validation prix négatif"
git commit -m "docs: mise à jour README"
git commit -m "test: ajout tests CategoryService"
```

### Branches

```
main     → Production (stable)
dev      → Développement (features intégrées)
feature/ → Nouvelles fonctionnalités
fix/     → Corrections de bugs
```

### Pull Requests

1. Créer une branche depuis `dev`
2. Développer la fonctionnalité
3. Écrire les tests
4. Créer une PR vers `dev`
5. Attendre les checks CI (tests + SonarQube)
6. Merge après validation

---

**Prochaine section** : [06 - Annexes](06-annexes.md)
