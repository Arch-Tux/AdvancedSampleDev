# Fonctionnement - AdvancedSampleDev

## 4.1 Flux de données

### Création d'un produit
```
Client HTTP
    │
    ▼
[ProductController] (API)
    │
    ▼
[ProductService] (Application)
    │
    ├─► Crée Price (Domain)
    ├─► Crée Product (Domain)
    │
    ▼
[IProductRepository] (Interface Domain)
    │
    ▼
[ProductRepository] (Infrastructure)
    │
    ▼
Base de données
```

### Changement de prix
```
Client HTTP
    │
    ▼
[ProductController]
    │
    ▼
[ProductService]
    │
    ├─► GetByIdAsync() → Product
    ├─► Product.ChangePrice(newPrice) → Validation métier
    │
    ▼
[ProductRepository.UpdateAsync()]
    │
    ▼
Base de données
```

## 4.2 Règles métier

### Gestion des prix
1. Un prix doit toujours être positif (> 0)
2. Un prix est composé d'un montant HT et d'une TVA
3. Le calcul TTC est automatique via la TVA
4. Un produit ne peut changer de prix que s'il est actif

### Gestion de la TVA
1. Trois taux uniquement : 5.5%, 10%, 20% (France)
2. Taux hardcodés et immuables (Singleton)
3. Calculs : HT → TTC et TTC → HT

### Gestion des fournisseurs
1. Un fournisseur doit avoir un nom non vide
2. Un produit peut avoir plusieurs fournisseurs
3. L'association produit-fournisseur est gérée par la couche Application/Infrastructure

## 4.3 Gestion des erreurs

### DomainException
Exception personnalisée pour les violations de règles métier :
- Prix invalide (≤ 0 ou null)
- TVA null
- Nom de fournisseur vide
- Tentative de changement de prix sur produit inactif

**Principe** : Fail-fast - les erreurs sont détectées au plus tôt (constructeurs, méthodes métier)

### Exemple de gestion d'erreur
```csharp
// Dans le constructeur de Price
public Price(decimal amountHt, Tva tva)
{
    AmountHt = amountHt > 0 
      ? amountHt 
      : throw new DomainException("Le prix HT doit être supérieur à zéro.");
    
    Tva = tva ?? throw new DomainException("La TVA ne peut pas être null.");
}
```

---

[← Retour à la documentation principale](./README.md)
