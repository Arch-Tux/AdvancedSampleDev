# Docker - PostgreSQL

## Démarrage rapide

### 1. Démarrer PostgreSQL
```bash
docker-compose up -d
```

### 2. Vérifier que le conteneur est démarré
```bash
docker-compose ps
```

### 3. Voir les logs
```bash
docker-compose logs -f database
```

### 4. Arrêter PostgreSQL
```bash
docker-compose down
```

### 5. Arrêter et supprimer les données
```bash
docker-compose down -v
```

## Configuration

### Variables d'environnement

Les credentials sont définis dans le fichier `.env` à la racine du projet (non commité dans Git).

Variables requises :
- **POSTGRES_HOST** - Hôte de la base de données (ex: localhost)
- **POSTGRES_USER** - Nom d'utilisateur PostgreSQL
- **POSTGRES_PASSWORD** - Mot de passe PostgreSQL
- **POSTGRES_DB** - Nom de la base de données
- **POSTGRES_PORT** - Port PostgreSQL (par défaut 5432)

Voir le fichier `.env` ou `docker-compose.yml` pour les valeurs actuelles.

### Connection String

La connection string est construite dynamiquement à partir des variables d'environnement du fichier `.env`.

Format :
```
Host={POSTGRES_HOST};Port={POSTGRES_PORT};Database={POSTGRES_DB};Username={POSTGRES_USER};Password={POSTGRES_PASSWORD}
```

⚠️ **Ne jamais committer de credentials en dur dans le code ou la documentation !**

## Commandes utiles

### Se connecter à PostgreSQL en ligne de commande
```bash
# Charger les variables d'environnement depuis .env
source .env

# Se connecter
docker exec -it advancedsampledev-postgres psql -U $POSTGRES_USER -d $POSTGRES_DB
```

### Exécuter une commande SQL
```bash
docker exec -it advancedsampledev-postgres psql -U $POSTGRES_USER -d $POSTGRES_DB -c "SELECT version();"
```

### Backup de la base de données
```bash
docker exec -t advancedsampledev-postgres pg_dump -U $POSTGRES_USER $POSTGRES_DB > backup.sql
```

### Restaurer un backup
```bash
docker exec -i advancedsampledev-postgres psql -U $POSTGRES_USER $POSTGRES_DB < backup.sql
```

## Healthcheck

Le conteneur PostgreSQL inclut un healthcheck qui vérifie toutes les 10 secondes que la base de données est prête.

Vérifier le status :
```bash
docker inspect advancedsampledev-postgres | grep -A 5 Health
```

## Volumes

Les données sont persistées dans un volume Docker nommé `postgres_data`.

Pour lister les volumes :
```bash
docker volume ls
```

Pour inspecter le volume :
```bash
docker volume inspect advancedsampledev_postgres_data
```

## Réseau

Le conteneur utilise un réseau bridge personnalisé `advancedsampledev-network` pour isoler la communication.

## Production

⚠️ **Important** : Ne jamais utiliser ce mot de passe en production !

Pour la production :
1. Utiliser des secrets (Docker Secrets, Kubernetes Secrets, Azure Key Vault, etc.)
2. Utiliser des mots de passe forts et aléatoires
3. Configurer SSL/TLS
4. Restreindre l'accès réseau
