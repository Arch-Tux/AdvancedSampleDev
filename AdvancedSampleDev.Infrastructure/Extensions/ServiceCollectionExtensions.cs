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
            return $"Data Source={configPath}";
        }
        
        // 3. Fallback final : Chemin par défaut basé sur AppContext.BaseDirectory
        var defaultDbPath = Path.Combine(AppContext.BaseDirectory, "advancedsampledev.db");
        return $"Data Source={defaultDbPath}";
    }
}
