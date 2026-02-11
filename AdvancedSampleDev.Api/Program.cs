using AdvancedSampleDev.Infrastructure.Extensions;
using AdvancedSampleDev.Application.Products;
using AdvancedSampleDev.Application.Suppliers;
using AdvancedSampleDev.domain.Interfaces.Product;
using AdvancedSampleDev.domain.Interfaces.Supplier;
using AdvancedSampleDev.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuration de la base de données SQLite
builder.Services.AddSqliteDbContext();

// Enregistrement des repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

// Enregistrement des services de la couche Application
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SupplierService>();

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

