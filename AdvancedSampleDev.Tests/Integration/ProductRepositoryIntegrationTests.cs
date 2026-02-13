using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.Infrastructure.Repositories;
using FluentAssertions;
using Xunit;

namespace AdvancedSampleDev.Tests.Integration;

public class ProductRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly ProductRepository _repository;

    public ProductRepositoryIntegrationTests()
    {
        _repository = new ProductRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistProduct()
    {
        // Arrange
        var product = new Product("Test Product", new Price(100m, TvaType.Standard));

        // Act
        await _repository.AddAsync(product);
        var result = await _repository.GetByIdAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Product");
        result.Price.GetAmountHt().Should().Be(100m);
        result.Price.GetTvaType().Should().Be(TvaType.Standard);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var product1 = new Product("Product 1", new Price(100m, TvaType.Standard));
        var product2 = new Product("Product 2", new Price(200m, TvaType.Reduced));
        await _repository.AddAsync(product1);
        await _repository.AddAsync(product2);

        // Act
        var results = await _repository.GetAllAsync();

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain(p => p.Name == "Product 1");
        results.Should().Contain(p => p.Name == "Product 2");
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnProduct()
    {
        // Arrange
        var product = new Product("Test Product", new Price(150m, TvaType.Intermediate));
        await _repository.AddAsync(product);

        // Act
        var result = await _repository.GetByIdAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(product.Id);
        result.Name.Should().Be("Test Product");
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyProduct()
    {
        // Arrange
        var product = new Product("Original Name", new Price(100m, TvaType.Standard));
        await _repository.AddAsync(product);
        
        // Détacher l'entité pour éviter le conflit de tracking
        Context.ChangeTracker.Clear();
        
        // Recréer l'instance avec le même ID
        var productToUpdate = Product.Reconstitute(
            product.Id, 
            "Original Name", 
            new Price(100m, TvaType.Standard),
            true,
            null
        );
        productToUpdate.ChangeName("Updated Name");
        productToUpdate.ChangePrice(new Price(200m, TvaType.Reduced));

        // Act
        await _repository.UpdateAsync(productToUpdate);
        
        // Détacher à nouveau pour la vérification
        Context.ChangeTracker.Clear();
        var result = await _repository.GetByIdAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated Name");
        result.Price.GetAmountHt().Should().Be(200m);
        result.Price.GetTvaType().Should().Be(TvaType.Reduced);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveProduct()
    {
        // Arrange
        var product = new Product("To Delete", new Price(100m, TvaType.Standard));
        await _repository.AddAsync(product);

        // Act
        await _repository.DeleteAsync(product.Id);
        var result = await _repository.GetByIdAsync(product.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var product = new Product("Test Product", new Price(100m, TvaType.Standard));
        await _repository.AddAsync(product);

        // Act
        var exists = await _repository.ExistsAsync(product.Id);

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var exists = await _repository.ExistsAsync(nonExistingId);

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task AddAsync_WithInactiveProduct_ShouldPersistCorrectState()
    {
        // Arrange
        var product = new Product("Inactive Product", new Price(100m, TvaType.Standard));
        product.Deactivate();

        // Act
        await _repository.AddAsync(product);
        var result = await _repository.GetByIdAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result!.GetIsActive().Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ChangingActiveState_ShouldPersist()
    {
        // Arrange
        var product = new Product("Test Product", new Price(100m, TvaType.Standard));
        await _repository.AddAsync(product);
        
        // Détacher l'entité pour éviter le conflit de tracking
        Context.ChangeTracker.Clear();
        
        // Recréer l'instance avec le même ID
        var productToUpdate = Product.Reconstitute(
            product.Id,
            "Test Product",
            new Price(100m, TvaType.Standard),
            true,
            null
        );
        productToUpdate.Deactivate();

        // Act
        await _repository.UpdateAsync(productToUpdate);
        
        // Détacher à nouveau pour la vérification
        Context.ChangeTracker.Clear();
        var result = await _repository.GetByIdAsync(product.Id);

        // Assert
        result.Should().NotBeNull();
        result!.GetIsActive().Should().BeFalse();
    }
}
