# AdvancedSampleDev CLI

Outil en ligne de commande pour gérer la base de données SQLite du projet AdvancedSampleDev.

## Prérequis

- .NET 10.0 SDK
- Aucune dépendance externe (SQLite est intégré)

## Commandes disponibles

### 🌱 Seed - Peupler la base de données

Remplit la base de données avec des données de test (20 suppliers, 100 produits).

```bash
dotnet run --project AdvancedSampleDev.Cli seed
```

### 🔧 DB Create - Créer la base de données

Crée la base de données et les tables si elles n'existent pas.

```bash
dotnet run --project AdvancedSampleDev.Cli db-create
```

### 🗑️ DB Drop - Supprimer la base de données

⚠️ **ATTENTION** : Supprime complètement la base de données et toutes les données.

```bash
dotnet run --project AdvancedSampleDev.Cli db-drop
```

### 🔄 DB Reset - Réinitialiser la base de données

Supprime la base de données, la recrée et effectue le seed. **Recommandé pour initialiser le projet**.

```bash
dotnet run --project AdvancedSampleDev.Cli db-reset
```

### 📚 Help - Afficher l'aide

```bash
dotnet run --project AdvancedSampleDev.Cli help
```

## Utilisation depuis la racine du projet

```bash
# Reset complet (recommandé au premier lancement)
dotnet run --project AdvancedSampleDev.Cli db-reset

# Seed uniquement
dotnet run --project AdvancedSampleDev.Cli seed
```

## Emplacement de la base de données

La base de données SQLite `advancedsampledev.db` est automatiquement créée **à la racine du projet** (où se trouve le fichier `.sln`).

- 📂 CLI et API partagent la même base de données
- ✅ Portable : fonctionne sur toutes les machines
- 🔍 Le fichier est automatiquement localisé, quel que soit le répertoire d'exécution

## Notes

- La base de données est créée automatiquement au premier lancement si elle n'existe pas
- Le seed ne s'exécute QUE si vous le demandez explicitement
- Utilisez `db-reset` pour repartir de zéro avec des données fraîches

