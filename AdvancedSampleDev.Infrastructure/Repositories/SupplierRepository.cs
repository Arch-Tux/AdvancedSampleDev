using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.domain.Interfaces.Supplier;
using AdvancedSampleDev.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedSampleDev.Infrastructure.Repositories;

public class SupplierRepository(ApplicationDbContext context) : ISupplierRepository
{
    public async Task<Supplier?> GetByIdAsync(Guid id)
    {
        var entity = await context.Suppliers.FindAsync(id);
        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        var entities = await context.Suppliers.ToListAsync();
        return entities.Select(MapToDomain);
    }

    public async Task AddAsync(Supplier entity)
    {
        var supplierEntity = MapToEntity(entity);
        await context.Suppliers.AddAsync(supplierEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Supplier entity)
    {
        var supplierEntity = MapToEntity(entity);
        context.Suppliers.Update(supplierEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.Suppliers.FindAsync(id);
        if (entity != null)
        {
            context.Suppliers.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Suppliers.AnyAsync(s => s.Id == id);
    }

    // Mapping Domain -> Entity
    private static SupplierEntity MapToEntity(Supplier supplier)
    {
        return new SupplierEntity
        {
            Id = supplier.Id,
            Name = supplier.Name
        };
    }

    // Mapping Entity -> Domain
    private static Supplier MapToDomain(SupplierEntity entity)
    {
        return new Supplier(entity.Name);
    }
}
