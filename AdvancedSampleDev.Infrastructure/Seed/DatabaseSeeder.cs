using AdvancedSampleDev.Infrastructure.Entities;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace AdvancedSampleDev.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Vérifier si des données existent déjà
        if (await context.Products.AnyAsync() || await context.Suppliers.AnyAsync())
        {
            return; // Déjà seedé
        }

        // Générer 20 suppliers
        var supplierFaker = new Faker<SupplierEntity>("fr")
            .RuleFor(s => s.Id, _ => Guid.NewGuid())
            .RuleFor(s => s.Name, f => f.Company.CompanyName());

        var suppliers = supplierFaker.Generate(20);
        await context.Suppliers.AddRangeAsync(suppliers);
        await context.SaveChangesAsync();

        // Générer 100 produits
        var tvaRates = new[] { 0.055m, 0.10m, 0.20m }; // 5.5%, 10%, 20%
        
        var productFaker = new Faker<ProductEntity>("fr")
            .RuleFor(p => p.Id, _ => Guid.NewGuid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.PriceHt, f => f.Finance.Amount(min: 5, max: 1000))
            .RuleFor(p => p.TvaRate, f => f.PickRandom(tvaRates))
            .RuleFor(p => p.IsActive, f => f.Random.Bool(0.9f)); // 90% actifs

        var products = productFaker.Generate(100);
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // Associer aléatoirement des suppliers aux produits (1 à 3 suppliers par produit)
        var random = new Random();
        var productSuppliers = new List<ProductSupplierEntity>();

        foreach (var product in products)
        {
            // Chaque produit a entre 1 et 3 fournisseurs
            var numberOfSuppliers = random.Next(1, 4);
            var selectedSuppliers = suppliers
                .OrderBy(_ => random.Next())
                .Take(numberOfSuppliers)
                .ToList();

            foreach (var supplier in selectedSuppliers)
            {
                productSuppliers.Add(new ProductSupplierEntity
                {
                    ProductId = product.Id,
                    SupplierId = supplier.Id
                });
            }
        }

        await context.ProductSuppliers.AddRangeAsync(productSuppliers);
        await context.SaveChangesAsync();
    }
}
