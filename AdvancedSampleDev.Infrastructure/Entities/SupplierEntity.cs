namespace AdvancedSampleDev.Infrastructure.Entities;

public class SupplierEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Navigation properties pour la relation many-to-many
    public ICollection<ProductSupplierEntity> ProductSuppliers { get; set; } = new List<ProductSupplierEntity>();
}
