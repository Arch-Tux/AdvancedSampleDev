using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.domain.Interfaces.Product;

namespace AdvancedSampleDev.Application.Products;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task<Product> CreateAsync(string name, Price price)
    {
        var product = new Product(name, price);
        await _productRepository.AddAsync(product);
        return product;
    }

    public async Task<Product?> UpdateAsync(Guid id, string name, Price price)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }

        product.ChangeName(name);
        product.ChangePrice(price);
        
        await _productRepository.UpdateAsync(product);
        return product;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exists = await _productRepository.ExistsAsync(id);
        if (!exists)
        {
            return false;
        }

        await _productRepository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> ActivateAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        product.Activate();
        await _productRepository.UpdateAsync(product);
        return true;
    }

    public async Task<bool> DeactivateAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        product.Deactivate();
        await _productRepository.UpdateAsync(product);
        return true;
    }
}

