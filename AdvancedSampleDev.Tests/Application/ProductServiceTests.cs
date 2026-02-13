using AdvancedSampleDev.Application.Products;
using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.domain.Interfaces.Product;
using FluentAssertions;
using Moq;
using Xunit;

namespace AdvancedSampleDev.Tests.Application;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _mockRepository = new Mock<IProductRepository>();
        _service = new ProductService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product("Produit 1", new Price(100m, TvaType.Standard)),
            new Product("Produit 2", new Price(200m, TvaType.Reduced))
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(products);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product("Produit Test", new Price(100m, TvaType.Standard));
        _mockRepository.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);

        // Act
        var result = await _service.GetByIdAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(product);
        _mockRepository.Verify(r => r.GetByIdAsync(productId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetByIdAsync(productId);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(productId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var name = "Nouveau Produit";
        var price = new Price(100m, TvaType.Standard);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(name, price);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.Price.Should().Be(price);
        result.GetIsActive().Should().BeTrue();
        _mockRepository.Verify(r => r.AddAsync(It.Is<Product>(p => p.Name == name)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithExistingProduct_ShouldUpdateAndReturnProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var existingProduct = Product.Reconstitute(
            productId,
            "Ancien nom",
            new Price(100m, TvaType.Standard),
            true,
            null
        );
        var newName = "Nouveau nom";
        var newPrice = new Price(200m, TvaType.Reduced);

        _mockRepository.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(existingProduct);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(productId, newName, newPrice);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(newName);
        result.Price.Should().Be(newPrice);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingProduct_ShouldReturnNull()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync((Product?)null);

        // Act
        var result = await _service.UpdateAsync(productId, "Nom", new Price(100m, TvaType.Standard));

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingProduct_ShouldReturnTrue()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _mockRepository.Setup(r => r.ExistsAsync(productId)).ReturnsAsync(true);
        _mockRepository.Setup(r => r.DeleteAsync(productId)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(productId);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(productId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingProduct_ShouldReturnFalse()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _mockRepository.Setup(r => r.ExistsAsync(productId)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(productId);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task ActivateAsync_WithExistingProduct_ShouldActivateAndReturnTrue()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = Product.Reconstitute(
            productId,
            "Produit",
            new Price(100m, TvaType.Standard),
            false, // Inactif
            null
        );
        _mockRepository.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.ActivateAsync(productId);

        // Assert
        result.Should().BeTrue();
        product.GetIsActive().Should().BeTrue();
        _mockRepository.Verify(r => r.UpdateAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeactivateAsync_WithExistingProduct_ShouldDeactivateAndReturnTrue()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product("Produit", new Price(100m, TvaType.Standard));
        _mockRepository.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeactivateAsync(productId);

        // Assert
        result.Should().BeTrue();
        product.GetIsActive().Should().BeFalse();
        _mockRepository.Verify(r => r.UpdateAsync(product), Times.Once);
    }
}
