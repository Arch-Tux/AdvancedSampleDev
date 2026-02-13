using AdvancedSampleDev.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace AdvancedSampleDev.Tests.Integration;

/// <summary>
/// Classe de base pour les tests d'intégration avec une base SQLite en mémoire
/// </summary>
public abstract class IntegrationTestBase : IDisposable
{
    protected readonly ApplicationDbContext Context;
    private readonly DbConnection _connection;

    protected IntegrationTestBase()
    {
        // Créer une connexion SQLite en mémoire
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        // Configurer le DbContext avec SQLite en mémoire
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new ApplicationDbContext(options);
        
        // Créer le schéma de base de données
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
