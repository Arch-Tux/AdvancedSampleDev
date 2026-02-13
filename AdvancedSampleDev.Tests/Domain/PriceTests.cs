using AdvancedSampleDev.domain.Entities;
using AdvancedSampleDev.domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace AdvancedSampleDev.Tests.Domain;

public class PriceTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreatePrice()
    {
        // Arrange
        var amountHt = 100m;
        var tvaType = TvaType.Standard;

        // Act
        var price = new Price(amountHt, tvaType);

        // Assert
        price.GetAmountHt().Should().Be(100m);
        price.GetTvaType().Should().Be(TvaType.Standard);
        price.GetTvaRate().Should().Be(0.20m);
    }

    [Fact]
    public void GetAmountTtc_WithStandardTva_ShouldCalculateCorrectly()
    {
        // Arrange
        var price = new Price(100m, TvaType.Standard);

        // Act
        var amountTtc = price.GetAmountTtc();

        // Assert
        amountTtc.Should().Be(120m); // 100 * 1.20
    }

    [Fact]
    public void GetAmountTtc_WithReducedTva_ShouldCalculateCorrectly()
    {
        // Arrange
        var price = new Price(100m, TvaType.Reduced);

        // Act
        var amountTtc = price.GetAmountTtc();

        // Assert
        amountTtc.Should().Be(105.5m); // 100 * 1.055
    }

    [Fact]
    public void GetAmountTtc_WithIntermediateTva_ShouldCalculateCorrectly()
    {
        // Arrange
        var price = new Price(100m, TvaType.Intermediate);

        // Act
        var amountTtc = price.GetAmountTtc();

        // Assert
        amountTtc.Should().Be(110m); // 100 * 1.10
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(0)]
    public void Constructor_WithNegativeOrZeroAmount_ShouldThrowDomainException(decimal invalidAmount)
    {
        // Arrange & Act
        Action act = () => new Price(invalidAmount, TvaType.Standard);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("Le prix HT doit être supérieur à zéro.");
    }

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var price1 = new Price(100m, TvaType.Standard);
        var price2 = new Price(100m, TvaType.Standard);

        // Act
        var result = price1.Equals(price2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentAmounts_ShouldReturnFalse()
    {
        // Arrange
        var price1 = new Price(100m, TvaType.Standard);
        var price2 = new Price(200m, TvaType.Standard);

        // Act
        var result = price1.Equals(price2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentTvaTypes_ShouldReturnFalse()
    {
        // Arrange
        var price1 = new Price(100m, TvaType.Standard);
        var price2 = new Price(100m, TvaType.Reduced);

        // Act
        var result = price1.Equals(price2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldBeEqual()
    {
        // Arrange
        var price1 = new Price(100m, TvaType.Standard);
        var price2 = new Price(100m, TvaType.Standard);

        // Act & Assert
        price1.GetHashCode().Should().Be(price2.GetHashCode());
    }
}
