# Configuration de l'application

## Fichiers de configuration

### `appsettings.json`
Fichier de configuration de base avec des placeholders. **Commité dans Git**.
Ne contient pas de credentials réels.

### `appsettings.Development.json`
Fichier de configuration pour l'environnement de développement local. **Commité dans Git**.
Contient les credentials de la base de données Docker locale (non sensibles).

### `appsettings.Production.json`
Fichier de configuration pour l'environnement de production. **Non commité dans Git** (dans .gitignore).
Doit contenir les vrais credentials de production.

## Configuration de la base de données

### Développement local
Les credentials de développement sont dans `appsettings.Development.json` et correspondent au `docker-compose.yml` :
- **Host**: localhost
- **Port**: 5432
- **Database**: advancedsampledev_db
- **Username**: advancedsampledev
- **Password**: DevPassword123!

### Production
Créez un fichier `appsettings.Production.json` avec vos credentials de production :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=YOUR_HOST;Port=5432;Database=YOUR_DATABASE;Username=YOUR_USER;Password=YOUR_PASSWORD"
  }
}
```

Ce fichier sera automatiquement ignoré par Git.
