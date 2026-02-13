using AdvancedSampleDev.Api.Features.Auth;
using AdvancedSampleDev.Api.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AdvancedSampleDev.Tests.Integration;

/// <summary>
/// Tests d'intégration pour l'authentification JWT
/// </summary>
public class AuthenticationTests
{
    [Fact]
    public void TokenService_GenerateToken_ShouldCreateValidToken()
    {
        // Arrange
        var configuration = new Dictionary<string, string?>
        {
            ["Jwt:SecretKey"] = "VotreCleSuperSecreteQuiDoitEtreTresLongue!MinimumMinimum32Caracteres",
            ["Jwt:Issuer"] = "AdvancedSampleDev.Api",
            ["Jwt:Audience"] = "AdvancedSampleDev.Client",
            ["Jwt:ExpirationInMinutes"] = "60"
        };

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection(configuration);
        var config = configBuilder.Build();

        var tokenService = new TokenService(config);

        // Act
        var token = tokenService.GenerateToken("testuser", "Admin");

        // Assert
        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3); // JWT a 3 parties: header.payload.signature
    }

    [Fact]
    public void LoginRequest_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var loginRequest = new LoginRequest("admin", "password");

        // Assert
        loginRequest.Username.Should().Be("admin");
        loginRequest.Password.Should().Be("password");
    }

    [Fact]
    public void LoginResponse_ShouldContainTokenAndExpiration()
    {
        // Arrange
        var expiresAt = DateTime.UtcNow.AddHours(1);
        
        // Act
        var response = new LoginResponse("fake-token", expiresAt);

        // Assert
        response.Token.Should().Be("fake-token");
        response.ExpiresAt.Should().Be(expiresAt);
    }
}
