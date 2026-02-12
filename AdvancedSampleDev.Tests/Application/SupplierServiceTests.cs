using AdvancedSampleDev.Application.Suppliers;
using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.domain.Interfaces.Supplier;
using FluentAssertions;
using Moq;
using Xunit;

namespace AdvancedSampleDev.Tests.Application;

public class SupplierServiceTests
{
    private readonly Mock<ISupplierRepository> _mockRepository;
    private readonly SupplierService _service;

    public SupplierServiceTests()
    {
        _mockRepository = new Mock<ISupplierRepository>();
        _service = new SupplierService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSuppliers()
    {
        // Arrange
        var suppliers = new List<Supplier>
        {
            new Supplier("Fournisseur 1"),
            new Supplier("Fournisseur 2")
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(suppliers);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(suppliers);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnSupplier()
    {
        // Arrange
        var supplierId = Guid.NewGuid();
        var supplier = new Supplier("Fournisseur Test");
        _mockRepository.Setup(r => r.GetByIdAsync(supplierId)).ReturnsAsync(supplier);

        // Act
        var result = await _service.GetByIdAsync(supplierId);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(supplier);
        _mockRepository.Verify(r => r.GetByIdAsync(supplierId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        var supplierId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(supplierId)).ReturnsAsync((Supplier?)null);

        // Act
        var result = await _service.GetByIdAsync(supplierId);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(supplierId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidName_ShouldCreateSupplier()
    {
        // Arrange
        var name = "Nouveau Fournisseur";
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Supplier>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(name);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        _mockRepository.Verify(r => r.AddAsync(It.Is<Supplier>(s => s.Name == name)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithExistingSupplier_ShouldUpdateAndReturnSupplier()
    {
        // Arrange
        var supplierId = Guid.NewGuid();
        var existingSupplier = Supplier.Reconstitute(supplierId, "Ancien nom");
        var newName = "Nouveau nom";

        _mockRepository.Setup(r => r.GetByIdAsync(supplierId)).ReturnsAsync(existingSupplier);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Supplier>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(supplierId, newName);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be(newName);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Supplier>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingSupplier_ShouldReturnNull()
    {
        // Arrange
        var supplierId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(supplierId)).ReturnsAsync((Supplier?)null);

        // Act
        var result = await _service.UpdateAsync(supplierId, "Nom");

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Supplier>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingSupplier_ShouldReturnTrue()
    {
        // Arrange
        var supplierId = Guid.NewGuid();
        _mockRepository.Setup(r => r.ExistsAsync(supplierId)).ReturnsAsync(true);
        _mockRepository.Setup(r => r.DeleteAsync(supplierId)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(supplierId);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(supplierId), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingSupplier_ShouldReturnFalse()
    {
        // Arrange
        var supplierId = Guid.NewGuid();
        _mockRepository.Setup(r => r.ExistsAsync(supplierId)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(supplierId);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }
}
