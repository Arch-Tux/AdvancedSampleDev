using AdvancedSampleDev.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedSampleDev.Api.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _configuration = configuration;
    }

    /// <summary>
    /// Authentifie un utilisateur et retourne un token JWT
    /// </summary>
    /// <remarks>
    /// Pour ce démo, utilisez : username = "admin", password = "password"
    /// </remarks>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // TODO: En production, valider contre une vraie base de données avec hash de mot de passe
        // Pour ce démo, on accepte username="admin" et password="password"
        if (request.Username != "admin" || request.Password != "password")
        {
            return Unauthorized(new { message = "Nom d'utilisateur ou mot de passe incorrect" });
        }

        var role = request.Username == "admin" ? "Admin" : "User";
        var token = _tokenService.GenerateToken(request.Username, role);
        var expirationMinutes = int.Parse(_configuration["Jwt:ExpirationInMinutes"] ?? "60");
        var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

        return Ok(new LoginResponse(token, expiresAt));
    }

    /// <summary>
    /// Endpoint protégé pour tester l'authentification
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        var username = User.Identity?.Name;
        var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

        return Ok(new
        {
            username,
            role,
            claims = User.Claims.Select(c => new { c.Type, c.Value })
        });
    }
}
