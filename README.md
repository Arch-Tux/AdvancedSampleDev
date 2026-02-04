# AdvancedSampleDev

Projet .NET avec architecture Clean Architecture (Domain, Application, Infrastructure, API).

## Démarrage rapide

### 1. Prérequis

- .NET 10.0 SDK
- Docker (pour PostgreSQL)
- Fichier `.env` à la racine du projet

### 2. Configuration

Créez un fichier `.env` à la racine avec :

```env
POSTGRES_HOST=localhost
POSTGRES_USER=advancedsampledev
POSTGRES_PASSWORD=DevPassword123!
POSTGRES_DB=advancedsampledev_db
POSTGRES_PORT=5432
```

### 3. Démarrer la base de données

```bash
docker-compose up -d database
```

### 4. Initialiser la base de données (première fois)

```bash
# Créer les tables et seeder avec des données de test
dotnet run --project AdvancedSampleDev.Cli db-reset
```

### 5. Lancer l'API

```bash
dotnet run --project AdvancedSampleDev.Api
```

L'API sera accessible sur : `http://localhost:5155`

## Outils CLI

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

## Architecture

- **Domain** : Entités métier, Value Objects, Interfaces
- **Application** : Logique métier, Services
- **Infrastructure** : Repositories, DbContext, Seed
- **Api** : Controllers, Configuration
- **Cli** : Outils de gestion de la base de données

## TODO

[ ] Utiliser encapsulation, héritage et polymorphisme dans un projet orienté objet  
[ ] pour les tests, utiliser le principe AAA (arrange, act, assert)
