using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdvancedSampleDev.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure le DbContext avec SQLite
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <param name="configuration">La configuration de l'application</param>
    /// <returns>La collection de services pour le chaînage</returns>
    public static IServiceCollection AddSqliteDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = GetSqliteConnectionString(configuration);
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        
        return services;
    }
    
    /// <summary>
    /// Construit la connection string SQLite à partir de la configuration ou du chemin par défaut
    /// </summary>
    private static string GetSqliteConnectionString(IConfiguration configuration)
    {
        // 1. Priorité : ConnectionString complète dans la configuration
        var configConnectionString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(configConnectionString))
        {
            return configConnectionString;
        }
        
        // 2. Fallback : Chemin configuré via DatabasePath
        var configPath = configuration["DatabasePath"];
        if (!string.IsNullOrWhiteSpace(configPath))
        {
            // Si c'est un chemin relatif, on le résout à partir de la racine du projet
            if (!Path.IsPathRooted(configPath))
            {
                var projectRoot = FindProjectRoot();
                if (projectRoot != null)
                {
                    configPath = Path.Combine(projectRoot, configPath);
                }
            }
            return $"Data Source={configPath}";
        }
        
        // 3. Fallback final : Racine du projet + advancedsampledev.db
        var projectRootFallback = FindProjectRoot();
        var defaultDbPath = projectRootFallback != null 
            ? Path.Combine(projectRootFallback, "advancedsampledev.db")
            : Path.Combine(AppContext.BaseDirectory, "advancedsampledev.db");
        
        return $"Data Source={defaultDbPath}";
    }
    
    /// <summary>
    /// Trouve la racine du projet en remontant jusqu'au fichier .sln
    /// </summary>
    private static string? FindProjectRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        
        while (directory != null)
        {
            // Cherche un fichier .sln dans le répertoire courant
            if (directory.GetFiles("*.sln").Length > 0)
            {
                return directory.FullName;
            }
            
            directory = directory.Parent;
        }
        
        return null;
    }
}
