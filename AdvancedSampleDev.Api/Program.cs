using AdvancedSampleDev.Infrastructure.Extensions;
using DotNetEnv;

// Charger les variables d'environnement depuis le fichier .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuration de la base de données PostgreSQL via l'extension centralisée
builder.Services.AddPostgreSqlDbContext();

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

