# Contexte - AdvancedSampleDev

## 2.1 Vue d'ensemble du projet
**AdvancedSampleDev** est une application backend basée sur une architecture en couches (Clean Architecture / DDD) pour la gestion d'un catalogue de produits avec gestion des prix, de la TVA et des fournisseurs.

## 2.2 Technologies utilisées
- **Framework** : .NET 8.0 / ASP.NET Core
- **Langage** : C# 12
- **Architecture** : Domain-Driven Design (DDD) + Clean Architecture
- **API** : REST API avec OpenAPI/Swagger

## 2.3 Objectifs métier
- Gérer un catalogue de produits avec prix et TVA
- Assurer la validité des données métier via encapsulation et invariants
- Gérer les relations produits-fournisseurs
- Respecter les principes SOLID, KISS, DRY, DDD, Clean architecture et Clean Code

## 2.4 Principes de conception appliqués

### SOLID
- **S**ingle Responsibility : Chaque classe a une seule responsabilité
- **O**pen/Closed : Ouvert à l'extension, fermé à la modification
- **L**iskov Substitution : Les sous-types doivent être substituables
- **I**nterface Segregation : Interfaces spécifiques plutôt que générales
- **D**ependency Inversion : Dépendre des abstractions, pas des implémentations

### Autres principes
- **KISS** (Keep It Simple, Stupid) : Simplicité avant tout
- **DRY** (Don't Repeat Yourself) : Éviter la duplication de code
- **DDD** : Modélisation centrée sur le domaine métier
- **Clean Architecture** : Séparation claire des couches (Domain, Application, Infrastructure, API)
- **Clean Code** : Lisibilité, maintenabilité, tests unitaires
- **Encapsulation** : Validation et invariants au niveau du domaine
- **Immutabilité** : Value Objects immuables (Price, Tva)

---

[← Retour à la documentation principale](./README.md)
