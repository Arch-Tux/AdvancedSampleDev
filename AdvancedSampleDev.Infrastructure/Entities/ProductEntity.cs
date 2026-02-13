namespace AdvancedSampleDev.Infrastructure.Entities;

using AdvancedSampleDev.domain.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PriceHt { get; set; }
    public TvaType TvaType { get; set; }
    public bool IsActive { get; set; }
    
    // Navigation properties pour la relation many-to-many
    public ICollection<ProductSupplierEntity> ProductSuppliers { get; set; } = new List<ProductSupplierEntity>();
}
