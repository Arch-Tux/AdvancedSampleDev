using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace AdvancedSampleDev.Tests.Domain;

public class SupplierTests
{
    [Fact]
    public void Constructor_WithValidName_ShouldCreateSupplier()
    {
        // Arrange
        var name = "Fournisseur Test";

        // Act
        var supplier = new Supplier(name);

        // Assert
        supplier.Name.Should().Be(name);
        supplier.Id.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowDomainException(string? invalidName)
    {
        // Arrange & Act
        Action act = () => new Supplier(invalidName);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Le nom du fournisseur ne peut pas être vide.");
    }

    [Fact]
    public void ChangeName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var supplier = new Supplier("Ancien nom");
        var newName = "Nouveau nom";

        // Act
        supplier.ChangeName(newName);

        // Assert
        supplier.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangeName_WithInvalidName_ShouldThrowDomainException(string? invalidName)
    {
        // Arrange
        var supplier = new Supplier("Nom valide");

        // Act
        Action act = () => supplier.ChangeName(invalidName);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Le nom du fournisseur ne peut pas être vide.");
    }

    [Fact]
    public void Reconstitute_WithValidData_ShouldCreateSupplierWithGivenId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Fournisseur Test";

        // Act
        var supplier = Supplier.Reconstitute(id, name);

        // Assert
        supplier.Id.Should().Be(id);
        supplier.Name.Should().Be(name);
    }

    [Fact]
    public void Reconstitute_WithInvalidName_ShouldThrowDomainException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        Action act = () => Supplier.Reconstitute(id, "");

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Le nom du fournisseur ne peut pas être vide.");
    }
}
