using AdvancedSampleDev.Infrastructure.Extensions;
using AdvancedSampleDev.Application.Products;
using AdvancedSampleDev.Application.Suppliers;
using AdvancedSampleDev.domain.Interfaces.Product;
using AdvancedSampleDev.domain.Interfaces.Supplier;
using AdvancedSampleDev.Infrastructure.Repositories;
using Scalar.AspNetCore;
using AdvancedSampleDev.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuration de la base de données SQLite
builder.Services.AddSqliteDbContext(builder.Configuration);

// Enregistrement des repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

// Enregistrement des services de la couche Application
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SupplierService>();

// Enregistrement du service de tokens JWT
builder.Services.AddScoped<ITokenService, TokenService>();

// Configuration de l'authentification JWT
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"] 
    ?? throw new InvalidOperationException("JWT SecretKey non configurée");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] 
    ?? throw new InvalidOperationException("JWT Issuer non configuré");
var jwtAudience = builder.Configuration["Jwt:Audience"] 
    ?? throw new InvalidOperationException("JWT Audience non configurée");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ClockSkew = TimeSpan.Zero // Pas de délai de grâce pour l'expiration
    };
});

builder.Services.AddAuthorization();

// Configuration OpenAPI native .NET 10
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Interface UI moderne de .NET 10
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

