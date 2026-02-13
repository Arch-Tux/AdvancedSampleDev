# 01 - Introduction

## 🎯 Présentation du projet

**AdvancedSampleDev** est une application .NET démontrant les bonnes pratiques de développement avec une architecture Clean Architecture.

### Objectifs

- Démontrer une **architecture propre et maintenable**
- Implémenter les **principes SOLID et DDD**
- Fournir une **API REST sécurisée avec JWT**
- Appliquer les **bonnes pratiques de tests** (unitaires + intégration)
- Intégrer une **CI/CD complète** (GitHub Actions + SonarQube)

---

## 📚 Table des matières

### 📖 Documentation principale

#### **[01 - Introduction](01-introduction.md)** (ce document)
Présentation du projet, stack technique, démarrage rapide

#### **[02 - Contexte métier](02-contexte.md)**
- Domaine d'application
- Entités métier (Product, Supplier, Price)
- Règles métier et validation
- Cas d'usage
- Concepts DDD appliqués

#### **[03 - Architecture](03-architecture.md)**
- Clean Architecture
- Structure du projet (7 projets)
- Couches (Domain, Application, Infrastructure, API)
- Flux de dépendances
- Principes SOLID

#### **[04 - Fonctionnement](04-fonctionnement.md)**
- Flow d'une requête HTTP
- Authentification JWT
- Mapping entre couches
- Cycle de vie des objets (DI)
- Diagrammes de séquence

#### **[05 - Procédures](05-procedures.md)**
- Installation et démarrage
- Authentification
- Exécution des tests
- Gestion de la base de données
- Tester l'API (cURL, Postman, Scalar)
- Guide développement
- Dépannage

#### **[06 - Annexes](06-annexes.md)**
- Ressources et références
- Outils recommandés
- Structure de la base de données
- Endpoints API complets
- Exemples de requêtes
- Checklist sécurité
- Performances et déploiement

### 🔧 Documentation technique spécialisée

#### **[07 - Authentification JWT](07-jwt-authentication.md)**
- Comment obtenir un token JWT
- Utiliser le token dans les requêtes
- Durée de validité (60 minutes)
- Renouvellement du token
- Endpoints d'authentification
- Exemples cURL et Scalar

#### **[08 - Couverture de code](08-code-coverage.md)**
- Script run-coverage.sh
- Configuration Coverlet
- Commandes de couverture (dotnet test --collect)
- Lecture des rapports HTML
- Métriques (Line, Branch, Method)
- Intégration CI/CD et Codecov

#### **[09 - Docker PostgreSQL](09-docker-postgresql.md)**
- ⚠️ Obsolète - Le projet utilise maintenant SQLite
- Configuration Docker Compose historique
- Commandes PostgreSQL (docker-compose, psql)
- Variables d'environnement
- Migration vers SQLite effectuée

#### **[10 - Scalar UI](10-scalar-ui.md)**
- Interface API interactive moderne (.NET 10)
- Comment tester les endpoints
- Swagger UI vs Scalar
- Configuration et personnalisation
- Guide d'utilisation complet

---

## 🏗️ Stack technique

### Backend
- **.NET 10.0** - Framework principal
- **ASP.NET Core** - API REST
- **Entity Framework Core** - ORM
- **SQLite** - Base de données (fichier local)

### Authentification
- **JWT (JSON Web Tokens)** - Authentification stateless
- **Bearer Token** - Sécurisation des endpoints

### Tests
- **xUnit** - Framework de tests
- **FluentAssertions** - Assertions lisibles
- **Moq** - Mocking
- **SQLite In-Memory** - Tests d'intégration

### CI/CD
- **GitHub Actions** - Automatisation
- **SonarQube Cloud** - Qualité de code
- **Codecov** - Couverture de tests

### Documentation
- **Scalar UI** - Interface interactive de l'API
- **Markdown** - Documentation technique

---

## 📊 Métriques du projet

- **7 projets** (.csproj)
- **~85 tests** (unitaires + intégration)
- **Couverture > 90%** (lignes de code)
- **0 vulnérabilités** de sécurité
- **Clean Code** (SonarQube Quality Gate)

---

## 🎓 Concepts démontrés

### Architecture
- ✅ Clean Architecture (Domain, Application, Infrastructure, API)
- ✅ Separation of Concerns
- ✅ Dependency Inversion

### Domain-Driven Design
- ✅ Entities avec invariants
- ✅ Value Objects (Price, TvaType)
- ✅ Aggregates
- ✅ Repository Pattern

### Bonnes pratiques
- ✅ SOLID principles
- ✅ Service Layer
- ✅ DTO Pattern
- ✅ Middleware Pattern
- ✅ Dependency Injection

### Sécurité
- ✅ JWT Authentication
- ✅ Secrets en variables d'environnement
- ✅ HTTPS
- ✅ Input validation

---

## 🚀 Démarrage rapide

```bash
# 1. Cloner le projet
git clone https://github.com/Arch-Tux/AdvancedSampleDev.git
cd AdvancedSampleDev

# 2. Configurer l'environnement
cp .env.example .env

# 3. Initialiser la base de données
dotnet run --project AdvancedSampleDev.Cli db-reset

# 4. Lancer l'API
dotnet run --project AdvancedSampleDev.Api
```

**L'API est accessible sur** : http://localhost:5155  
**Interface Scalar UI** : http://localhost:5155/scalar/v1

---

## 🎯 Guides par thème

### 👨‍💻 Pour les débutants
1. Lire **[01 - Introduction](01-introduction.md)** (ce document)
2. Suivre **[05 - Procédures > Installation](05-procedures.md#installation-et-démarrage)**
3. Tester l'API avec **[10 - Scalar UI](10-scalar-ui.md)**
4. Consulter les **[06 - Annexes > Exemples](06-annexes.md#exemples-de-requêtes)**

### 🏛️ Pour comprendre l'architecture
1. Lire **[03 - Architecture](03-architecture.md)**
2. Lire **[04 - Fonctionnement](04-fonctionnement.md)**
3. Consulter les diagrammes

### 🏢 Pour le contexte métier
1. Lire **[02 - Contexte métier](02-contexte.md)**
2. Comprendre les règles de TVA
3. Étudier les invariants des entités

### 🔐 Pour l'authentification
1. Lire **[07 - Authentification JWT](07-jwt-authentication.md)**
2. Consulter **[05 - Procédures > Authentification](05-procedures.md#authentification)**
3. Tester le login dans Scalar UI

### 🧪 Pour les tests et couverture
1. Lire **[05 - Procédures > Tests](05-procedures.md#exécuter-les-tests)**
2. Lire **[08 - Couverture de code](08-code-coverage.md)**
3. Exécuter le script `./run-coverage.sh`

### 🛠️ Pour développer
1. Lire **[05 - Procédures > Développement](05-procedures.md#développement)**
2. Suivre les conventions de nommage
3. Écrire des tests
4. Vérifier avec SonarQube

---

## 📄 Licence

Ce projet est un exemple éducatif. Libre d'utilisation et de modification.

---

**Prochaine section** : [02 - Contexte métier](02-contexte.md)

