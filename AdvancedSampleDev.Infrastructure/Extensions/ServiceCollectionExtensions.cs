using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AdvancedSampleDev.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configure le DbContext avec SQLite
    /// </summary>
    /// <param name="services">La collection de services</param>
    /// <returns>La collection de services pour le chaînage</returns>
    public static IServiceCollection AddSqliteDbContext(this IServiceCollection services)
    {
        // Chercher la racine du projet (où se trouve le .sln)
        var solutionRoot = FindSolutionRoot(Directory.GetCurrentDirectory());
        var dbPath = solutionRoot != null 
            ? Path.Combine(solutionRoot, "advancedsampledev.db")
            : "advancedsampledev.db";
        
        var connectionString = $"Data Source={dbPath}";
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        
        return services;
    }
    
    // Helper pour trouver la racine de la solution (où se trouve le fichier .sln)
    private static string? FindSolutionRoot(string startDirectory)
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
}
