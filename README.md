# AdvancedSampleDev

Projet .NET avec architecture Clean Architecture (Domain, Application, Infrastructure, API) utilisant SQLite.

## Démarrage rapide

### 1. Prérequis

- .NET 10.0 SDK

**C'est tout ! Pas besoin de Docker, PostgreSQL, ou autre. 🎉**

### 2. Configuration de la clé JWT

**⚠️ IMPORTANT pour l'authentification**

Créer un fichier `.env` à la racine du projet :

```bash
cp .env.example .env
```

Le fichier `.env` contient la clé secrète JWT :

```bash
# JWT Secret Key (minimum 32 caractères)
JWT_SECRET_KEY=
```

**📌 Notes de sécurité :**
- ✅ Le fichier `.env` est dans `.gitignore` (ne sera pas commité)
- ⚠️ En production : utiliser des secrets managers (Azure Key Vault, AWS Secrets Manager)
- 🔑 Générer une clé aléatoire forte (min. 32 caractères)

### 3. Initialiser la base de données

```bash
# Créer les tables et seeder avec des données de test
dotnet run --project AdvancedSampleDev.Cli db-reset
```

### 4. Lancer l'API

```bash
dotnet run --project AdvancedSampleDev.Api
```

L'API sera accessible sur : `http://localhost:5155`

### 5. Authentification JWT (IMPORTANT)

🔐 **L'API est protégée par JWT. Tous les endpoints nécessitent un token valide.**

#### Étape 1 : Générer un token

**Dans un terminal, exécute :**

```bash
curl -X POST http://localhost:5155/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'
```

**Réponse :**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2026-02-13T16:30:00Z"
}
```

**Copie le token**

#### Étape 2 : Utiliser le token

**Dans Scalar UI ou tout autre client HTTP, ajoute ce header à CHAQUE requête :**

- **Key** : `Authorization`
- **Value** : `Bearer eyJhbGci...` (**N'oublie pas l'espace après "Bearer"**)

**Exemple avec cURL :**

```bash
curl -X GET http://localhost:5155/api/products \
  -H "Authorization: Bearer <ton-token-ici>"
```

**⚠️ Infos importantes :**
- ⏱️ Le token expire après **60 minutes**
- 🔄 Après expiration : Refaire un appel à `/api/auth/login`
- 📚 Documentation complète : `Docs/jwt-authentication.md`

### 6. Tester l'API avec Scalar UI

Ouvre ton navigateur sur : **http://localhost:5155/scalar/v1**

Scalar UI te permet de :
- ✅ Visualiser tous les endpoints
- ✅ Tester les requêtes directement depuis le navigateur
- ✅ Voir la documentation interactive
- ✅ Interface moderne et rapide (recommandée pour .NET 10)
- ✅ Pas besoin de Postman !

Documentation complète : `Docs/scalar-ui.md`

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

### 🔓 Authentification (Public)
- `POST /api/auth/login` - Obtenir un token JWT
- `GET /api/auth/me` - Vérifier son token (requiert JWT)

### 🔒 Products (Protégés par JWT)
- `GET /api/products` - Liste tous les produits
- `GET /api/products/{id}` - Récupère un produit
- `POST /api/products` - Crée un produit
- `PUT /api/products/{id}` - Met à jour un produit
- `DELETE /api/products/{id}` - Supprime un produit
- `PATCH /api/products/{id}/activate` - Active un produit
- `PATCH /api/products/{id}/deactivate` - Désactive un produit

### 🔒 Suppliers (Protégés par JWT)
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
