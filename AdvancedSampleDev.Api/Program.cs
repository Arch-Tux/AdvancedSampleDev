using AdvancedSampleDev.Infrastructure.Extensions;
using AdvancedSampleDev.Application.Services;
using AdvancedSampleDev.domain.Interfaces.Product;
using AdvancedSampleDev.domain.Interfaces.Supplier;
using AdvancedSampleDev.Infrastructure.Repositories;
using DotNetEnv;

// Charger les variables d'environnement depuis le fichier .env à la racine de la solution
var solutionRoot = FindSolutionRoot(Directory.GetCurrentDirectory());
if (solutionRoot != null)
{
    var envPath = Path.Combine(solutionRoot, ".env");
    if (File.Exists(envPath))
    {
        Env.Load(envPath);
    }
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuration de la base de données PostgreSQL via l'extension centralisée
builder.Services.AddPostgreSqlDbContext();

// Enregistrement des repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

// Enregistrement des services de la couche Application
builder.Services.AddScoped<ProductService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

// Helper pour trouver la racine de la solution (où se trouve le fichier .sln)
static string? FindSolutionRoot(string startDirectory)
{
    var directory = new DirectoryInfo(startDirectory);
    while (directory != null)
    {
        if (directory.GetFiles("*.sln").Length > 0)
        {
            return directory.FullName;
        }
        directory = directory.Parent;
    }
    return null;
}
