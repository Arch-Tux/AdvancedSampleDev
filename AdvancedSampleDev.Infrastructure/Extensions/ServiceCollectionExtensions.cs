using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdvancedSampleDev.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure le DbContext avec PostgreSQL en utilisant les variables d'environnement
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour le chaînage</returns>
    /// <exception cref="InvalidOperationException">Si les variables d'environnement requises ne sont pas définies</exception>
    public static IServiceCollection AddPostgreSqlDbContext(this IServiceCollection services)
    {
        var connectionString = BuildConnectionStringFromEnvironment();
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        return services;
    }
    
    /// <summary>
    /// Construit la connection string PostgreSQL à partir des variables d'environnement
    /// </summary>
    /// <returns>La connection string</returns>
    /// <exception cref="InvalidOperationException">Si les variables d'environnement requises ne sont pas définies</exception>
    public static string BuildConnectionStringFromEnvironment()
    {
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
        var database = Environment.GetEnvironmentVariable("POSTGRES_DB") 
            ?? throw new InvalidOperationException("POSTGRES_DB non défini dans les variables d'environnement");
        var username = Environment.GetEnvironmentVariable("POSTGRES_USER") 
            ?? throw new InvalidOperationException("POSTGRES_USER non défini dans les variables d'environnement");
        var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") 
            ?? throw new InvalidOperationException("POSTGRES_PASSWORD non défini dans les variables d'environnement");

        return $"Host={host};Port={port};Database={database};Username={username};Password={password}";
    }
}
