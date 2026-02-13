# 02 - Contexte métier

## 🏢 Domaine d'application

Le projet simule un **système de gestion de catalogue produits** avec fournisseurs.

---

## 📦 Entités métier

### Product (Produit)

**Attributs :**
- `Id` : Guid - Identifiant unique
- `Name` : string - Nom du produit (obligatoire, non vide)
- `Price` : Price - Prix avec TVA
- `IsActive` : bool - État du produit

**Comportements :**
- `Activate()` / `Deactivate()` - Gestion de l'état
- `ChangeName(string)` - Modification du nom
- `ChangePrice(Price)` - Modification du prix

**Invariants :**
- ✅ Le nom ne peut pas être vide
- ✅ Le prix doit toujours être valide (> 0)
- ✅ Un produit peut être actif ou inactif

---

### Supplier (Fournisseur)

**Attributs :**
- `Id` : Guid - Identifiant unique
- `Name` : string - Nom du fournisseur (obligatoire, non vide)

**Comportements :**
- `ChangeName(string)` - Modification du nom

**Invariants :**
- ✅ Le nom ne peut pas être vide

---

### Price (Value Object)

**Attributs :**
- `AmountHT` : decimal - Montant hors taxes
- `TvaType` : TvaType - Type de TVA applicable

**Comportements :**
- `GetAmountTTC()` - Calcul du montant TTC
- `GetAmountHT()` - Récupération du montant HT

**Invariants :**
- ✅ Le montant HT doit être > 0
- ✅ Le type de TVA doit être valide

**Types de TVA :**
- `Reduced` : 5.5% (produits essentiels)
- `Intermediate` : 10% (restauration, transport)
- `Standard` : 20% (taux normal)

---

## 🎯 Règles métier

### Gestion des prix

1. **Tous les prix sont stockés HT**
   - Le TTC est calculé à la demande
   - La TVA dépend du type de produit

2. **TVA française**
   - Taux réduit : 5.5%
   - Taux intermédiaire : 10%
   - Taux normal : 20%

3. **Validation**
   - Un prix ne peut jamais être négatif ou zéro
   - Un prix ne peut pas changer de type de TVA une fois créé

### Gestion des produits

1. **Création**
   - Un produit doit avoir un nom et un prix valide
   - Par défaut, un produit est actif

2. **Activation/Désactivation**
   - Un produit peut être désactivé (soft delete)
   - Un produit désactivé peut être réactivé

3. **Modification**
   - Le nom peut être modifié
   - Le prix peut être modifié (nouveau montant, même TVA)

### Gestion des fournisseurs

1. **Création**
   - Un fournisseur doit avoir un nom non vide

2. **Relations**
   - Un produit peut avoir 0, 1 ou plusieurs fournisseurs
   - Un fournisseur peut fournir 0, 1 ou plusieurs produits

---

## 🔐 Règles de sécurité

### Authentification

1. **Tous les endpoints sont protégés par JWT**
   - Seul `/api/auth/login` est public
   - Un token valide est requis pour toutes les autres opérations

2. **Expiration des tokens**
   - Durée de vie : 60 minutes
   - Pas de refresh token (pour simplifier)
   - Nécessité de se reconnecter après expiration

3. **Compte de test**
   - Username : `admin`
   - Password : `password`
   - Rôle : `Admin`

---

## 📊 Cas d'usage

### UC1 : Créer un produit

**Acteur :** Utilisateur authentifié

**Préconditions :**
- L'utilisateur possède un token JWT valide

**Flux principal :**
1. L'utilisateur envoie une requête POST `/api/products`
2. Le système valide les données (nom, prix, TVA)
3. Le système crée le produit avec un ID unique
4. Le système retourne le produit créé (201 Created)

**Règles :**
- Le nom ne doit pas être vide
- Le prix doit être > 0
- Le type de TVA doit être valide

---

### UC2 : Lister les produits

**Acteur :** Utilisateur authentifié

**Flux principal :**
1. L'utilisateur envoie une requête GET `/api/products`
2. Le système retourne tous les produits actifs
3. Chaque produit affiche son prix HT et TTC

---

### UC3 : Authentification

**Acteur :** Utilisateur non authentifié

**Flux principal :**
1. L'utilisateur envoie username + password à `/api/auth/login`
2. Le système valide les credentials
3. Le système génère un token JWT
4. Le système retourne le token + date d'expiration

**Flux alternatif :**
- Credentials invalides → 401 Unauthorized

---

## 🎓 Concepts DDD appliqués

### Entities
- `Product` et `Supplier` sont des **Entities**
- Identifiés par leur `Id` (Guid)
- Ont un cycle de vie
- Peuvent changer d'état

### Value Objects
- `Price` est un **Value Object**
- Immuable (pas de setters)
- Égalité par valeur, pas par identité
- Pas d'identifiant propre

### Aggregates
- `Product` est la racine d'agrégat
- Protège ses invariants
- Modifications uniquement via ses méthodes publiques

### Domain Services
- Aucun pour l'instant (logique simple)
- À ajouter si logique métier complexe entre entités

---

## 🔄 Évolutions futures possibles

### Fonctionnalités
- 🔲 Gestion du stock
- 🔲 Historique des prix
- 🔲 Catégories de produits
- 🔲 Multi-devises
- 🔲 Promotions et remises

### Technique
- 🔲 Event Sourcing
- 🔲 CQRS
- 🔲 Cache Redis
- 🔲 Message Queue (RabbitMQ)
- 🔲 Microservices

---

**Prochaine section** : [03 - Architecture](03-architecture.md)
