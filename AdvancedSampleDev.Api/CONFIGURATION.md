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

Pour le développement avec Docker (voir `docker-compose.yml`), utilisez :

```env
POSTGRES_HOST=localhost
POSTGRES_USER=advancedsampledev
POSTGRES_PASSWORD=DevPassword123!
POSTGRES_DB=advancedsampledev_db
POSTGRES_PORT=5432
```

### Production

Pour la production, modifiez simplement les valeurs dans votre fichier `.env` :

```env
POSTGRES_HOST=votre_serveur_prod
POSTGRES_USER=votre_user_prod
POSTGRES_PASSWORD=votre_password_prod
POSTGRES_DB=votre_db_prod
POSTGRES_PORT=5432
```

## Sécurité

⚠️ **IMPORTANT** :
- Le fichier `.env` est automatiquement ignoré par Git
- Ne committez **JAMAIS** de credentials en dur dans le code
- Utilisez uniquement le fichier `.env` pour les configurations sensibles
