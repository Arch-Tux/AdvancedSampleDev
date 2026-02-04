# Configuration de l'application

## Fichiers de configuration

### `appsettings.json`
Fichier de configuration de base **sans credentials**. Commité dans Git.
Contient uniquement les paramètres de logging et autres configurations génériques.

### `.env`
**Fichier de configuration principal pour les credentials**. **Non commité dans Git** (dans .gitignore).
Ce fichier contient toutes les variables d'environnement sensibles.

## Configuration de la base de données

### Variables d'environnement requises

Créez un fichier `.env` à la racine du projet avec les variables suivantes :

```env
POSTGRES_HOST=localhost
POSTGRES_USER=votre_utilisateur
POSTGRES_PASSWORD=votre_mot_de_passe
POSTGRES_DB=votre_base_de_donnees
POSTGRES_PORT=5432
```

### Développement local (Docker)

Pour le développement avec Docker, les valeurs sont définies dans `docker-compose.yml` et doivent être reportées dans votre fichier `.env` local.

Consultez le fichier `docker-compose.yml` pour voir les variables d'environnement requises.

### Production

Pour la production, modifiez simplement les valeurs dans votre fichier `.env` :

```env
POSTGRES_HOST=votre_serveur_prod
POSTGRES_USER=votre_user_prod
POSTGRES_PASSWORD=votre_password_prod
POSTGRES_DB=votre_db_prod
POSTGRES_PORT=5432
```

## Gestion de la base de données

### CLI dédié pour la gestion de la base (création/suppression/seed)

Un projet CLI dédié (`AdvancedSampleDev.Cli`) permet de gérer la base de données **indépendamment du lancement de l'application**.
Ce CLI n'exécute pas les migrations EF Core : il utilise uniquement `EnsureCreated` / `EnsureDeleted` ainsi que des scripts de seed.

**⚠️ Important** : Le seed ne s'exécute **plus automatiquement** au démarrage de l'application. Cela permet d'éviter :
- La création/altération non désirée de la base en production
- L'ajout de temps au cold start de l'application

### Commandes disponibles

```bash
# Seeder la base de données avec des données de test
dotnet run --project AdvancedSampleDev.Cli seed

# Créer la base de données
dotnet run --project AdvancedSampleDev.Cli db-create

# Réinitialiser complètement (drop + create + seed)
dotnet run --project AdvancedSampleDev.Cli db-reset

# Supprimer la base de données
dotnet run --project AdvancedSampleDev.Cli db-drop
```

Voir `AdvancedSampleDev.Cli/README.md` pour plus de détails.

## Sécurité

⚠️ **IMPORTANT** :
- Le fichier `.env` est automatiquement ignoré par Git
- Ne committez **JAMAIS** de credentials en dur dans le code
- Utilisez uniquement le fichier `.env` pour les configurations sensibles
