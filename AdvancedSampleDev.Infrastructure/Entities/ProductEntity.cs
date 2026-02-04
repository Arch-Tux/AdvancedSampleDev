namespace AdvancedSampleDev.Infrastructure.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal PriceHt { get; set; }
    public decimal TvaRate { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties pour la relation many-to-many
    public ICollection<ProductSupplierEntity> ProductSuppliers { get; set; } = new List<ProductSupplierEntity>();
}
