# AdvancedSampleDev

Projet .NET avec architecture Clean Architecture (Domain, Application, Infrastructure, API) utilisant SQLite.

## Démarrage rapide

### 1. Prérequis

- .NET 10.0 SDK

**C'est tout ! Pas besoin de Docker, PostgreSQL, ou autre. 🎉**

### 2. Initialiser la base de données

```bash
# Créer les tables et seeder avec des données de test
dotnet run --project AdvancedSampleDev.Cli db-reset
```

### 3. Lancer l'API

```bash
dotnet run --project AdvancedSampleDev.Api
```

L'API sera accessible sur : `http://localhost:5155`

### 4. Tester l'API avec Swagger UI

Ouvre ton navigateur sur : **http://localhost:5155/scalar**

Swagger UI te permet de :
- ✅ Visualiser tous les endpoints
- ✅ Tester les requêtes directement depuis le navigateur
- ✅ Voir la documentation interactive
- ✅ Pas besoin de Postman !

Documentation complète : `Docs/swagger-ui.md`

---

## 📂 Base de données

Le projet utilise **SQLite** avec un fichier local `advancedsampledev.db` créé automatiquement.

**Avantages :**
- ✅ Pas besoin de Docker
- ✅ Configuration zéro
- ✅ Fichier portable et partageable
- ✅ Données persistantes

---

## 🛠️ Outils CLI

Le projet inclut un CLI dédié pour gérer la base de données :

```bash
# Seeder la base
dotnet run --project AdvancedSampleDev.Cli seed

# Réinitialiser complètement
dotnet run --project AdvancedSampleDev.Cli db-reset

# Voir toutes les commandes
dotnet run --project AdvancedSampleDev.Cli help
```

Voir `AdvancedSampleDev.Cli/README.md` pour plus de détails.

---

## 🎯 Endpoints API

### Products
- `GET /api/products` - Liste tous les produits
- `GET /api/products/{id}` - Récupère un produit
- `POST /api/products` - Crée un produit
- `PUT /api/products/{id}` - Met à jour un produit
- `DELETE /api/products/{id}` - Supprime un produit
- `PATCH /api/products/{id}/activate` - Active un produit
- `PATCH /api/products/{id}/deactivate` - Désactive un produit

### Suppliers
- `GET /api/suppliers` - Liste tous les fournisseurs
- `GET /api/suppliers/{id}` - Récupère un fournisseur
- `POST /api/suppliers` - Crée un fournisseur
- `PUT /api/suppliers/{id}` - Met à jour un fournisseur
- `DELETE /api/suppliers/{id}` - Supprime un fournisseur

Documentation complète : `AdvancedSampleDev.Api/Features/*/README.md`

---

## 🏗️ Architecture

```
Domain/          → Entités métier, Value Objects, Interfaces
Application/     → Logique métier, Services, DTOs
Infrastructure/  → Repositories, DbContext, Seed
Api/             → Controllers, Configuration
Cli/             → Outils de gestion de la base de données
```

### Principes appliqués
- ✅ **Clean Architecture**
- ✅ **DDD** (Domain-Driven Design)
- ✅ **SOLID**
- ✅ **Vertical Slice** (organisation par feature)

---

## 📚 Documentation

- `Docs/migration-sqlite.md` - Migration PostgreSQL → SQLite
- `Docs/migration-tva-to-tvatype.md` - Migration Tva (singleton) → TvaType (enum)
- `Docs/sonarqube-configuration.md` - Configuration SonarQube Cloud

---

## TODO

[ ] Utiliser encapsulation, héritage et polymorphisme dans un projet orienté objet  
[ ] pour les tests, utiliser le principe AAA (arrange, act, assert)
