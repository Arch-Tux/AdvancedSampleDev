namespace AdvancedSampleDev.Api.Middlewares;

/// <summary>
/// Extension pour configurer tous les middlewares de l'application
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Configure le pipeline de middlewares dans l'ordre correct
    /// </summary>
    public static IApplicationBuilder UseAppMiddlewares(this IApplicationBuilder app)
    {
        // 1. Redirection HTTPS (sécurité)
        app.UseHttpsRedirection();

        // 2. Authentification JWT (qui es-tu ?)
        app.UseAuthentication();

        // 3. Autorisation (as-tu le droit ?)
        app.UseAuthorization();

        return app;
    }
}
