using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace AdvancedSampleDev.Tests.Domain;

public class ProductTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateProduct()
    {
        // Arrange
        var name = "Produit Test";
        var price = new Price(100m, TvaType.Standard);

        // Act
        var product = new Product(name, price);

        // Assert
        product.Name.Should().Be(name);
        product.Price.Should().Be(price);
        product.Id.Should().NotBeEmpty();
        product.GetIsActive().Should().BeTrue();
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldThrowDomainException(string? invalidName)
    {
        // Arrange
        var price = new Price(100m, TvaType.Standard);

        // Act
        Action act = () => new Product(invalidName!, price);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Le nom du produit ne peut pas être vide.");
    }

    [Fact]
    public void Constructor_WithNullPrice_ShouldThrowDomainException()
    {
        // Arrange & Act
        Action act = () => new Product("Produit", null!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Le prix ne peut pas être null.");
    }

    [Fact]
    public void ChangeName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var product = new Product("Ancien nom", new Price(100m, TvaType.Standard));
        var newName = "Nouveau nom";

        // Act
        product.ChangeName(newName);

        // Assert
        product.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null!)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangeName_WithInvalidName_ShouldThrowDomainException(string? invalidName)
    {
        // Arrange
        var product = new Product("Nom valide", new Price(100m, TvaType.Standard));

        // Act
        Action act = () => product.ChangeName(invalidName!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Le nom du produit ne peut pas être vide.");
    }

    [Fact]
    public void ChangePrice_WithValidPrice_ShouldUpdatePrice()
    {
        // Arrange
        var product = new Product("Produit", new Price(100m, TvaType.Standard));
        var newPrice = new Price(200m, TvaType.Reduced);

        // Act
        product.ChangePrice(newPrice);

        // Assert
        product.Price.Should().Be(newPrice);
    }

    [Fact]
    public void ChangePrice_WithNullPrice_ShouldThrowDomainException()
    {
        // Arrange
        var product = new Product("Produit", new Price(100m, TvaType.Standard));

        // Act
        Action act = () => product.ChangePrice(null!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Le prix ne peut pas être null.");
    }

    [Fact]
    public void Activate_WhenInactive_ShouldActivateProduct()
    {
        // Arrange
        var product = new Product("Produit", new Price(100m, TvaType.Standard));
        product.Deactivate();

        // Act
        product.Activate();

        // Assert
        product.GetIsActive().Should().BeTrue();
    }

    [Fact]
    public void Deactivate_WhenActive_ShouldDeactivateProduct()
    {
        // Arrange
        var product = new Product("Produit", new Price(100m, TvaType.Standard));

        // Act
        product.Deactivate();

        // Assert
        product.GetIsActive().Should().BeFalse();
    }

    [Fact]
    public void Reconstitute_WithValidData_ShouldCreateProductWithGivenId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Produit Test";
        var price = new Price(100m, TvaType.Standard);
        var isActive = false;
        var suppliers = new List<Supplier> { new Supplier("Fournisseur 1") };

        // Act
        var product = Product.Reconstitute(id, name, price, isActive, suppliers);

        // Assert
        product.Id.Should().Be(id);
        product.Name.Should().Be(name);
        product.Price.Should().Be(price);
        product.GetIsActive().Should().Be(isActive);
    }

    [Fact]
    public void Reconstitute_WithEmptySuppliersList_ShouldCreateProductWithoutSuppliers()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Produit Test";
        var price = new Price(100m, TvaType.Standard);

        // Act
        var product = Product.Reconstitute(id, name, price, true, null);

        // Assert
        product.Id.Should().Be(id);
        product.Name.Should().Be(name);
    }
}
