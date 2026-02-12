# Swagger UI - Documentation

## 🎯 Accès à Swagger UI

Une fois l'API lancée, Swagger UI est accessible à :

**URL : http://localhost:5155/scalar**

Swagger UI est configuré à la racine pour un accès direct et facile.

---

## 🚀 Démarrage

```bash
# 1. S'assurer que la base de données est créée
dotnet run --project AdvancedSampleDev.Cli db-reset

# 2. Lancer l'API
dotnet run --project AdvancedSampleDev.Api

# 3. Ouvrir dans le navigateur
open http://localhost:5155/scalar
# ou
# http://localhost:5155/scalar
```

---

## 📊 Fonctionnalités de Swagger UI

### Interface interactive

Swagger UI permet de :
- ✅ **Visualiser** tous les endpoints de l'API
- ✅ **Tester** les endpoints directement depuis le navigateur
- ✅ **Voir** les schémas de requêtes et réponses
- ✅ **Exécuter** des requêtes HTTP sans Postman
- ✅ **Télécharger** la spécification OpenAPI (JSON/YAML)

### Endpoints disponibles

#### **Products**
- `GET /api/products` - Liste tous les produits
- `GET /api/products/{id}` - Récupère un produit
- `POST /api/products` - Crée un produit
- `PUT /api/products/{id}` - Met à jour un produit
- `DELETE /api/products/{id}` - Supprime un produit
- `PATCH /api/products/{id}/activate` - Active un produit
- `PATCH /api/products/{id}/deactivate` - Désactive un produit

#### **Suppliers**
- `GET /api/suppliers` - Liste tous les fournisseurs
- `GET /api/suppliers/{id}` - Récupère un fournisseur
- `POST /api/suppliers` - Crée un fournisseur
- `PUT /api/suppliers/{id}` - Met à jour un fournisseur
- `DELETE /api/suppliers/{id}` - Supprime un fournisseur

---

## 🧪 Comment tester un endpoint

### Exemple : Créer un produit

1. **Ouvrir Swagger UI** : http://localhost:5155/scalar
2. **Cliquer sur** `POST /api/products`
3. **Cliquer sur** "Try it out"
4. **Modifier le JSON** :
   ```json
   {
     "name": "Mon nouveau produit",
     "priceHt": 99.99,
     "tvaType": "Standard"
   }
   ```
5. **Cliquer sur** "Execute"
6. **Voir la réponse** en bas avec le code HTTP 201 et l'objet créé

---

## 📝 Configuration Swagger

### Dans Program.cs

```csharp
// Configuration de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AdvancedSampleDev API",
        Version = "v1",
        Description = "API REST pour la gestion de produits et fournisseurs",
        Contact = new OpenApiContact
        {
            Name = "AdvancedSampleDev",
            Url = new Uri("https://github.com/Arch-Tux/AdvancedSampleDev")
        }
    });
    
    // Commentaires XML pour documentation enrichie
    options.IncludeXmlComments(xmlPath);
});

// Activation Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AdvancedSampleDev API v1");
        options.RoutePrefix = string.Empty; // Swagger à la racine
    });
}
```

---

## 📖 Documentation automatique

Grâce aux commentaires XML (/// dans le code), Swagger affiche :
- Description des endpoints
- Paramètres requis
- Types de réponses
- Codes HTTP possibles

### Exemple dans le contrôleur

```csharp
/// <summary>
/// Récupère tous les produits
/// </summary>
/// <returns>Liste des produits</returns>
[HttpGet]
[ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
{
    // ...
}
```

Swagger UI affichera automatiquement cette documentation ! 📚

---

## 🔗 Endpoints Swagger

### Swagger UI (Interface graphique)
```
http://localhost:5155/
```

### Spécification OpenAPI JSON
```
http://localhost:5155/swagger/v1/swagger.json
```

Tu peux importer ce JSON dans :
- **Postman** (Import → OpenAPI)
- **Insomnia**
- **REST Client** (VS Code)
- **Tout autre client API**

---

## ✨ Avantages de Swagger UI

### Pour le développement
1. ✅ **Pas besoin de Postman** pendant le dev
2. ✅ **Documentation toujours à jour** (auto-générée)
3. ✅ **Test rapide** des endpoints
4. ✅ **Découverte de l'API** intuitive

### Pour la livraison client
1. ✅ **Interface de test** incluse
2. ✅ **Documentation interactive**
3. ✅ **Pas besoin d'outils externes**
4. ✅ **Standard OpenAPI** (portable)

---

## 🎨 Personnalisation (optionnel)

### Changer le thème Swagger UI

```csharp
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = string.Empty;
    
    // Options supplémentaires
    options.DocumentTitle = "AdvancedSampleDev API";
    options.DefaultModelsExpandDepth(-1); // Cacher les modèles par défaut
    options.DocExpansion(DocExpansion.List); // Collapse par défaut
    options.EnableDeepLinking(); // Deep linking pour partager des URLs
});
```

---

## 🚨 En production

**⚠️ Attention :** Swagger est configuré pour **Development uniquement**

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(...);
}
```

En production, Swagger sera **désactivé** automatiquement pour des raisons de sécurité.

---

## 📸 Aperçu de l'interface

Quand tu ouvres http://localhost:5155/, tu verras :

```
┌────────────────────────────────────────┐
│  AdvancedSampleDev API v1              │
│  API REST pour la gestion de produits  │
│                                        │
│  ▼ Products                            │
│    GET    /api/products                │
│    POST   /api/products                │
│    GET    /api/products/{id}           │
│    PUT    /api/products/{id}           │
│    DELETE /api/products/{id}           │
│    PATCH  /api/products/{id}/activate  │
│    PATCH  /api/products/{id}/deactivate│
│                                        │
│  ▼ Suppliers                           │
│    GET    /api/suppliers               │
│    POST   /api/suppliers               │
│    ...                                 │
│                                        │
│  Schemas ▼                             │
│    ProductResponseDto                  │
│    CreateProductDto                    │
│    ...                                 │
└────────────────────────────────────────┘
```

---

## ✅ Résultat

**Ton client peut maintenant :**
1. ✅ Lancer l'API : `dotnet run --project AdvancedSampleDev.Api`
2. ✅ Ouvrir son navigateur : `http://localhost:5155/scalar`
3. ✅ **Tester toute l'API visuellement** sans écrire une seule ligne de code
4. ✅ Voir la documentation complète
5. ✅ Exporter la spec OpenAPI pour Postman si besoin

**Pas besoin de développer un front, Swagger UI fait office d'interface de test professionnelle ! 🎉**
