namespace AdvancedSampleDev.domain.Interfaces.Product;

public interface IProductRepository : IRepository<Entities.Product>
{
  Task<IEnumerable<Entities.Product>> GetBySupplierIdAsync(Guid supplierId);
}
