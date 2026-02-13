using AdvancedSampleDev.Infrastructure.Extensions;
using AdvancedSampleDev.Application.Products;
using AdvancedSampleDev.Application.Suppliers;
using AdvancedSampleDev.domain.Interfaces.Product;
using AdvancedSampleDev.domain.Interfaces.Supplier;
using AdvancedSampleDev.Infrastructure.Repositories;
using Scalar.AspNetCore;
using AdvancedSampleDev.Api.Services;
using AdvancedSampleDev.Api.Middlewares;

// Charger les variables d'environnement depuis le fichier .env
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuration de la base de données SQLite
builder.Services.AddSqliteDbContext(builder.Configuration);

// Enregistrement des repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

// Enregistrement des services de la couche Application
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SupplierService>();

// Enregistrement du service de tokens JWT
builder.Services.AddScoped<ITokenService, TokenService>();

// Configuration de l'authentification JWT (centralisée dans Middlewares)
builder.Services.AddJwtAuthentication(builder.Configuration);

// Configuration OpenAPI native .NET 10
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Interface UI moderne de .NET 10
}

// Configuration des middlewares (centralisée dans Middlewares)
app.UseAppMiddlewares();

app.MapControllers();

await app.RunAsync();

