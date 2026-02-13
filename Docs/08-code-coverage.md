# 📊 Couverture de Code - Guide

## 🎯 Commandes disponibles

### Méthode simple (avec script)
```bash
./run-coverage.sh
```

Ce script :
1. ✅ Exécute tous les tests avec couverture
2. ✅ Génère un rapport HTML
3. ✅ Affiche un résumé dans le terminal
4. ✅ Ouvre automatiquement le rapport dans le navigateur

---

### Méthode manuelle

#### 1. Exécuter les tests avec couverture
```bash
dotnet test --collect:"XPlat Code Coverage"
```

#### 2. Avec configuration personnalisée
```bash
dotnet test --settings coverlet.runsettings --collect:"XPlat Code Coverage"
```

#### 3. Générer le rapport HTML
```bash
# Installer reportgenerator (une fois)
dotnet tool install --global dotnet-reportgenerator-globaltool

# Générer le rapport
reportgenerator \
    -reports:"**/coverage.cobertura.xml" \
    -targetdir:"coveragereport" \
    -reporttypes:"Html"

# Ouvrir le rapport
open coveragereport/index.html
```

---

## 📂 Fichiers générés

### Après les tests
```
AdvancedSampleDev.Tests/
└── TestResults/
    └── {guid}/
        └── coverage.cobertura.xml  # Rapport de couverture brut
```

### Après génération du rapport
```
coveragereport/
├── index.html              # Page principale du rapport
├── Summary.txt             # Résumé texte
└── ...                     # Autres fichiers du rapport
```

---

## ⚙️ Configuration (coverlet.runsettings)

Le fichier `coverlet.runsettings` permet de configurer la couverture :

```xml
<?xml version="1.0" encoding="utf-8" ?>
<RunSettings>
  <DataCollectionRunSettings>
    <DataCollectors>
      <DataCollector friendlyName="XPlat Code Coverage">
        <Configuration>
          <!-- Formats de sortie -->
          <Format>cobertura,opencover</Format>
          
          <!-- Exclure les projets de tests -->
          <Exclude>[*.Tests]*</Exclude>
          
          <!-- Exclure les fichiers -->
          <ExcludeByFile>**/Migrations/**,**/obj/**,**/bin/**</ExcludeByFile>
          
          <!-- Exclure les attributs -->
          <ExcludeByAttribute>Obsolete,GeneratedCodeAttribute</ExcludeByAttribute>
        </Configuration>
      </DataCollector>
    </DataCollectors>
  </DataCollectionRunSettings>
</RunSettings>
```

### Options disponibles

#### Formats de sortie
```xml
<Format>cobertura,opencover,lcov,json</Format>
```

#### Exclusions par pattern
```xml
<Exclude>[AdvancedSampleDev.Tests]*,[*.Migrations]*</Exclude>
```

#### Exclusions par fichier
```xml
<ExcludeByFile>**/Migrations/**,**/Program.cs</ExcludeByFile>
```

#### Inclure uniquement certains assemblies
```xml
<Include>[AdvancedSampleDev.Domain]*,[AdvancedSampleDev.Application]*</Include>
```

---

## 📊 Lecture du rapport

### Métriques principales

#### Line Coverage (Couverture de lignes)
- **Pourcentage de lignes de code exécutées** pendant les tests
- 🎯 Objectif : > 80%

#### Branch Coverage (Couverture de branches)
- **Pourcentage de branches logiques testées** (if/else, switch, etc.)
- 🎯 Objectif : > 75%

#### Method Coverage (Couverture de méthodes)
- **Pourcentage de méthodes appelées** pendant les tests
- 🎯 Objectif : > 90%

### Code couleur dans le rapport HTML

- 🟢 **Vert** : Ligne couverte (exécutée par les tests)
- 🔴 **Rouge** : Ligne non couverte
- 🟡 **Jaune** : Branche partiellement couverte

---

## 🎯 Exemple de résumé

```
Summary
  Generated on: 2026-02-12 - 14:30:15
  Parser: Cobertura
  Assemblies: 3
  Classes: 12
  Files: 12
  Line coverage: 92.5%
  Covered lines: 185
  Uncovered lines: 15
  Coverable lines: 200
  Total lines: 450
  Branch coverage: 85.7%
  Covered branches: 24
  Total branches: 28
```

---

## 🔍 Analyser les résultats

### Identifier le code non couvert

1. Ouvrir `coveragereport/index.html`
2. Cliquer sur un assembly (ex: `AdvancedSampleDev.Domain`)
3. Voir les classes avec leur % de couverture
4. Cliquer sur une classe pour voir les lignes non couvertes en rouge

### Améliorer la couverture

#### Code non couvert typique :
- ✅ Getters/Setters privés
- ✅ Constructeurs de reconstitution
- ✅ Méthodes de validation
- ✅ Branches d'exception

#### Exemple de test manquant :
```csharp
// Code non couvert (branche else)
public void Activate()
{
    if (!IsActive)  // ✅ Testé
    {
        IsActive = true;
    }
    // ❌ Branche "déjà actif" non testée
}

// Test à ajouter
[Fact]
public void Activate_WhenAlreadyActive_ShouldRemainActive()
{
    // Arrange
    var product = new Product("Test", new Price(100m, TvaType.Standard));
    product.Activate();  // Déjà actif

    // Act
    product.Activate();  // Re-activer

    // Assert
    product.GetIsActive().Should().BeTrue();
}
```

---

## 📈 Formats de rapport

### HTML (recommandé)
```bash
reportgenerator -reporttypes:"Html"
```
✅ Interactif, détaillé, facile à naviguer

### Badges
```bash
reportgenerator -reporttypes:"Badges"
```
✅ Génère des badges SVG pour README.md

### Markdown
```bash
reportgenerator -reporttypes:"MarkdownSummary"
```
✅ Résumé en Markdown

### Tous les formats
```bash
reportgenerator -reporttypes:"Html;Badges;MarkdownSummary;TextSummary"
```

---

## 🚀 Intégration CI/CD

### GitHub Actions
```yaml
- name: Test with coverage
  run: dotnet test --collect:"XPlat Code Coverage"

- name: Generate coverage report
  run: |
    dotnet tool install --global dotnet-reportgenerator-globaltool
    reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport"

- name: Upload coverage
  uses: codecov/codecov-action@v3
  with:
    files: ./coveragereport/Cobertura.xml
```

### GitLab CI
```yaml
test:
  script:
    - dotnet test --collect:"XPlat Code Coverage"
  coverage: '/Line coverage: \d+\.\d+%/'
  artifacts:
    paths:
      - "**/TestResults/**/coverage.cobertura.xml"
```

---

## 📚 Outils complémentaires

### Coverlet (utilisé en arrière-plan)
- Collecteur de couverture pour .NET
- Intégré à `dotnet test`

### ReportGenerator
- Génère des rapports HTML depuis Cobertura/OpenCover
- Supporte 20+ formats de sortie

### Codecov / Coveralls
- Services cloud pour tracker la couverture dans le temps
- Intégration GitHub/GitLab

---

## ✅ Bonnes pratiques

### ✅ À faire
1. Viser **80%+ de line coverage**
2. Viser **75%+ de branch coverage**
3. Tester les **cas limites** et **exceptions**
4. **Exclure** le code généré de la couverture
5. **Tracker** l'évolution dans le temps

### ❌ À éviter
1. Ne pas viser 100% à tout prix
2. Ne pas tester les getters/setters simples
3. Ne pas tester le code auto-généré
4. Ne pas sacrifier la qualité des tests pour le %

---

## 🎓 Interpréter les résultats

### 90%+ Coverage
🟢 **Excellent** - Code bien testé

### 80-90% Coverage
🟡 **Bon** - Niveau acceptable pour la production

### 70-80% Coverage
🟠 **Moyen** - Amélioration recommandée

### <70% Coverage
🔴 **Faible** - Risque élevé de bugs

---

## 📊 Commandes utiles

### Couverture par projet
```bash
dotnet test AdvancedSampleDev.Tests --collect:"XPlat Code Coverage"
```

### Couverture avec filtres
```bash
dotnet test --collect:"XPlat Code Coverage" --filter "FullyQualifiedName~Domain"
```

### Couverture sans build
```bash
dotnet test --no-build --collect:"XPlat Code Coverage"
```

### Rapport avec seuil minimum
```bash
reportgenerator \
    -reports:"**/coverage.cobertura.xml" \
    -targetdir:"coveragereport" \
    -reporttypes:"Html" \
    -classfilters:"+*;-*.Tests.*"
```

---

## 🔗 Ressources

- [Coverlet Documentation](https://github.com/coverlet-coverage/coverlet)
- [ReportGenerator](https://github.com/danielpalme/ReportGenerator)
- [Microsoft - Code Coverage](https://learn.microsoft.com/dotnet/core/testing/unit-testing-code-coverage)
