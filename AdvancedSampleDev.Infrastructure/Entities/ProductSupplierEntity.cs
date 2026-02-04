namespace AdvancedSampleDev.Infrastructure.Entities;

/// <summary>
/// Table de jointure pour la relation many-to-many entre Product et Supplier
/// </summary>
public class ProductSupplierEntity
{
    public Guid ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    
    public Guid SupplierId { get; set; }
    public SupplierEntity Supplier { get; set; } = null!;
}
