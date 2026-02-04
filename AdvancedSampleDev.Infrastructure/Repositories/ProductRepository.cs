using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.domain.Interfaces.Product;
using AdvancedSampleDev.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedSampleDev.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var entity = await context.Products
            .Include(p => p.ProductSuppliers)
                .ThenInclude(ps => ps.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);

        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var entities = await context.Products
            .Include(p => p.ProductSuppliers)
                .ThenInclude(ps => ps.Supplier)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<Product>> GetBySupplierIdAsync(Guid supplierId)
    {
        var entities = await context.Products
            .Include(p => p.ProductSuppliers)
                .ThenInclude(ps => ps.Supplier)
            .Where(p => p.ProductSuppliers.Any(ps => ps.SupplierId == supplierId))
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task AddAsync(Product entity)
    {
        var productEntity = MapToEntity(entity);
        await context.Products.AddAsync(productEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product entity)
    {
        var productEntity = MapToEntity(entity);
        context.Products.Update(productEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.Products.FindAsync(id);
        if (entity != null)
        {
            context.Products.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Products.AnyAsync(p => p.Id == id);
    }

    // Mapping Domain -> Entity
    private static ProductEntity MapToEntity(Product product)
    {
        return new ProductEntity
        {
            Id = product.Id,
            Name = product.Name,
            PriceHt = product.Price.GetAmountHt(),
            TvaRate = product.Price.GetTvaRate(),
            IsActive = product.GetIsActive()
        };
    }

    // Mapping Entity -> Domain
    private static Product MapToDomain(ProductEntity entity)
    {
        var tva = GetTvaFromRate(entity.TvaRate);
        var price = new Price(entity.PriceHt, tva);
        
        // Mapper les suppliers associés
        var suppliers = entity.ProductSuppliers?
            .Where(ps => ps.Supplier != null)
            .Select(ps => Supplier.Reconstitute(ps.Supplier!.Id, ps.Supplier!.Name))
            .ToList();
        
        var product = Product.Reconstitute(
            entity.Id, 
            entity.Name, 
            price, 
            entity.IsActive, 
            suppliers);
        
        return product;
    }

    private static Tva GetTvaFromRate(decimal rate)
    {
        return rate switch
        {
            0.055m => Tva.Reduced,
            0.10m => Tva.Intermediate,
            0.20m => Tva.Standard,
            _ => Tva.Standard // Par défaut
        };
    }
}
