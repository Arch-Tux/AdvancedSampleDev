# Procédures - AdvancedSampleDev

## 5.1 Configuration de l'environnement de développement

### Prérequis
- .NET SDK 8.0 ou supérieur
- IDE : JetBrains Rider, Visual Studio 2022, ou VS Code
- Git

### Installation
```bash
# Cloner le repository
git clone https://github.com/Arch-Tux/AdvancedSampleDev.git
cd AdvancedSampleDev

# Restaurer les dépendances
dotnet restore

# Build de la solution
dotnet build
```

## 5.2 Exécution du projet

### Lancer l'API
```bash
cd AdvancedSampleDev.Api
dotnet run
```

L'API sera accessible sur : `https://localhost:5001` (ou port configuré)

### Swagger UI
Accessible en mode Development : `https://localhost:5001/openapi`

## 5.3 Tests

### Exécuter tous les tests
```bash
dotnet test
```

### Exécuter les tests d'un projet spécifique
```bash
dotnet test AdvancedSampleDev.Tests/AdvancedSampleDev.Tests.csproj
```

### Pattern AAA (Arrange, Act, Assert)
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

## 5.4 Contribution

### Workflow Git
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

### Conventions de nommage

#### Branches
- `feat-` : Nouvelle fonctionnalité
- `fix-` : Correction de bug
- `docs-` : Documentation
- `refactor-` : Refactoring sans changement fonctionnel
- `test-` : Ajout/modification de tests

#### Commits
Convention Conventional Commits :
- `feat:` Nouvelle fonctionnalité
- `fix:` Correction de bug
- `docs:` Documentation
- `refactor:` Refactoring sans changement fonctionnel
- `test:` Ajout/modification de tests
- `chore:` Tâches de maintenance

## 5.5 Déploiement

### Build en production
```bash
dotnet publish -c Release -o ./publish
```

### Variables d'environnement
À configurer selon l'environnement :
- `ASPNETCORE_ENVIRONMENT` : `Development`, `Staging`, `Production`
- Chaînes de connexion BDD (à venir)
- Secrets API (à venir)

## 5.6 Checklist de développement

### Avant de créer une Pull Request
- [ ] Le code compile sans erreur ni warning
- [ ] Les tests unitaires passent
- [ ] Le code respecte les principes SOLID
- [ ] L'encapsulation est respectée (pas de setters publics)
- [ ] Les exceptions métier utilisent `DomainException`
- [ ] La documentation est à jour
- [ ] Les commits suivent les conventions

### Revue de code
- [ ] Les règles métier sont dans le Domain
- [ ] Pas de logique métier dans l'API ou l'Infrastructure
- [ ] Les Value Objects sont immuables
- [ ] Les repositories utilisent les interfaces du Domain
- [ ] Pattern AAA respecté dans les tests

---

[← Retour à la documentation principale](./README.md)
