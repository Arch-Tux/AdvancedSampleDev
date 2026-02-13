using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.Infrastructure.Repositories;
using FluentAssertions;
using Xunit;

namespace AdvancedSampleDev.Tests.Integration;

public class SupplierRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly SupplierRepository _repository;

    public SupplierRepositoryIntegrationTests()
    {
        _repository = new SupplierRepository(Context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistSupplier()
    {
        // Arrange
        var supplier = new Supplier("Test Supplier");

        // Act
        await _repository.AddAsync(supplier);
        var result = await _repository.GetByIdAsync(supplier.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Supplier");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSuppliers()
    {
        // Arrange
        var supplier1 = new Supplier("Supplier 1");
        var supplier2 = new Supplier("Supplier 2");
        await _repository.AddAsync(supplier1);
        await _repository.AddAsync(supplier2);

        // Act
        var results = await _repository.GetAllAsync();

        // Assert
        results.Should().HaveCount(2);
        results.Should().Contain(s => s.Name == "Supplier 1");
        results.Should().Contain(s => s.Name == "Supplier 2");
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnSupplier()
    {
        // Arrange
        var supplier = new Supplier("Test Supplier");
        await _repository.AddAsync(supplier);

        // Act
        var result = await _repository.GetByIdAsync(supplier.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(supplier.Id);
        result.Name.Should().Be("Test Supplier");
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
    public async Task UpdateAsync_ShouldModifySupplier()
    {
        // Arrange
        var supplier = new Supplier("Original Name");
        await _repository.AddAsync(supplier);
        
        // Détacher l'entité pour éviter le conflit de tracking
        Context.ChangeTracker.Clear();
        
        // Recréer l'instance avec le même ID
        var supplierToUpdate = Supplier.Reconstitute(supplier.Id, "Original Name");
        supplierToUpdate.ChangeName("Updated Name");

        // Act
        await _repository.UpdateAsync(supplierToUpdate);
        
        // Détacher à nouveau pour la vérification
        Context.ChangeTracker.Clear();
        var result = await _repository.GetByIdAsync(supplier.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Updated Name");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveSupplier()
    {
        // Arrange
        var supplier = new Supplier("To Delete");
        await _repository.AddAsync(supplier);

        // Act
        await _repository.DeleteAsync(supplier.Id);
        var result = await _repository.GetByIdAsync(supplier.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        var supplier = new Supplier("Test Supplier");
        await _repository.AddAsync(supplier);

        // Act
        var exists = await _repository.ExistsAsync(supplier.Id);

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
}
