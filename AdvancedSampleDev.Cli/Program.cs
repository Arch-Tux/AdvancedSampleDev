﻿using AdvancedSampleDev.Infrastructure;
using AdvancedSampleDev.Infrastructure.Seed;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Charger les variables d'environnement depuis le fichier .env
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
}
else
{
    Env.Load(); // Tenter de charger depuis le répertoire courant
}

var builder = Host.CreateApplicationBuilder(args);

// Construction de la connection string à partir des variables d'environnement
var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
var database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? throw new InvalidOperationException("POSTGRES_DB non défini");
var username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? throw new InvalidOperationException("POSTGRES_USER non défini");
var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? throw new InvalidOperationException("POSTGRES_PASSWORD non défini");

var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

// Configuration de la base de données
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Récupération des arguments de commande
var command = args.Length > 0 ? args[0].ToLower() : "help";

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ApplicationDbContext>();

    switch (command)
    {
        case "seed":
            Console.WriteLine("🌱 Démarrage du seed de la base de données...");
            
            // Créer la base de données si elle n'existe pas
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("✅ Base de données créée ou déjà existante");
            
            // Seeder les données
            await DatabaseSeeder.SeedAsync(context);
            Console.WriteLine("✅ Seed terminé avec succès !");
            break;

        case "db-create":
            Console.WriteLine("🔧 Création de la base de données...");
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("✅ Base de données créée avec succès !");
            break;

        case "db-drop":
            Console.WriteLine("⚠️  Suppression de la base de données...");
            await context.Database.EnsureDeletedAsync();
            Console.WriteLine("✅ Base de données supprimée !");
            break;

        case "db-reset":
            Console.WriteLine("🔄 Réinitialisation de la base de données...");
            await context.Database.EnsureDeletedAsync();
            Console.WriteLine("✅ Base de données supprimée");
            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("✅ Base de données recréée");
            await DatabaseSeeder.SeedAsync(context);
            Console.WriteLine("✅ Seed effectué - Réinitialisation terminée !");
            break;

        default:
            Console.WriteLine("📚 CLI AdvancedSampleDev - Commandes disponibles:");
            Console.WriteLine();
            Console.WriteLine("  seed        - Seeder la base de données avec des données de test");
            Console.WriteLine("  db-create   - Créer la base de données et les tables");
            Console.WriteLine("  db-drop     - Supprimer la base de données");
            Console.WriteLine("  db-reset    - Supprimer, recréer et seeder la base de données");
            Console.WriteLine("  help        - Afficher cette aide");
            Console.WriteLine();
            Console.WriteLine("Exemples:");
            Console.WriteLine("  dotnet run --project AdvancedSampleDev.Cli seed");
            Console.WriteLine("  dotnet run --project AdvancedSampleDev.Cli db-reset");
            break;
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"❌ Erreur: {ex.Message}");
    Console.ResetColor();
    Environment.Exit(1);
}
