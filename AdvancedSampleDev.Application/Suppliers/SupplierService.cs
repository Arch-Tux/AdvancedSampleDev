using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.domain.Interfaces.Supplier;

namespace AdvancedSampleDev.Application.Suppliers;

public class SupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<Supplier?> GetByIdAsync(Guid id)
    {
        return await _supplierRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync()
    {
        return await _supplierRepository.GetAllAsync();
    }

    public async Task<Supplier> CreateAsync(string name)
    {
        var supplier = new Supplier(name);
        await _supplierRepository.AddAsync(supplier);
        return supplier;
    }

    public async Task<Supplier?> UpdateAsync(Guid id, string name)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null)
        {
            return null;
        }

        supplier.ChangeName(name);
        await _supplierRepository.UpdateAsync(supplier);
        
        return supplier;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exists = await _supplierRepository.ExistsAsync(id);
        if (!exists)
        {
            return false;
        }

        await _supplierRepository.DeleteAsync(id);
        return true;
    }
}
