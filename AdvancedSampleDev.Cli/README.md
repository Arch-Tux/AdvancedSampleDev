# AdvancedSampleDev CLI

Outil en ligne de commande pour gérer la base de données du projet AdvancedSampleDev.

## Prérequis

- Le fichier `.env` doit être présent à la racine du projet avec les variables de configuration PostgreSQL
- Docker doit être lancé avec PostgreSQL (via `docker-compose up -d database`)

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

Supprime la base de données, la recrée et effectue le seed. Utile pour repartir de zéro.

```bash
dotnet run --project AdvancedSampleDev.Cli db-reset
```

### 📚 Help - Afficher l'aide

```bash
dotnet run --project AdvancedSampleDev.Cli help
```

## Utilisation depuis la racine du projet

```bash
# Seed
dotnet run --project AdvancedSampleDev.Cli seed

# Reset complet
dotnet run --project AdvancedSampleDev.Cli db-reset
```

## Notes

- Le CLI charge automatiquement le fichier `.env` depuis la racine du projet
- Les commandes utilisent les mêmes configurations que l'API
- Le seed ne s'exécute QUE si vous le demandez explicitement (plus d'exécution automatique au démarrage de l'app)
