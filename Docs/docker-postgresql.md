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
- **POSTGRES_USER** : `advancedsampledev`
- **POSTGRES_PASSWORD** : `DevPassword123!`
- **POSTGRES_DB** : `advancedsampledev_db`
- **Port** : `5432`

### Connection String
```
Host=localhost;Port=5432;Database=advancedsampledev_db;Username=advancedsampledev;Password=DevPassword123!
```

## Commandes utiles

### Se connecter à PostgreSQL en ligne de commande
```bash
docker exec -it advancedsampledev-postgres psql -U advancedsampledev -d advancedsampledev_db
```

### Exécuter une commande SQL
```bash
docker exec -it advancedsampledev-postgres psql -U advancedsampledev -d advancedsampledev_db -c "SELECT version();"
```

### Backup de la base de données
```bash
docker exec -t advancedsampledev-postgres pg_dump -U advancedsampledev advancedsampledev_db > backup.sql
```

### Restaurer un backup
```bash
docker exec -i advancedsampledev-postgres psql -U advancedsampledev advancedsampledev_db < backup.sql
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
