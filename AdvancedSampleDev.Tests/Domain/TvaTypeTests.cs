using AdvancedSampleDev.domain.Entities;
using FluentAssertions;
using Xunit;

namespace AdvancedSampleDev.Tests.Domain;

public class TvaTypeTests
{
    [Theory]
    [InlineData(TvaType.Reduced, 0.055)]
    [InlineData(TvaType.Intermediate, 0.10)]
    [InlineData(TvaType.Standard, 0.20)]
    public void GetRate_ShouldReturnCorrectRate(TvaType tvaType, decimal expectedRate)
    {
        // Act
        var rate = tvaType.GetRate();

        // Assert
        rate.Should().Be(expectedRate);
    }

    [Theory]
    [InlineData(TvaType.Standard, 100, 120)]
    [InlineData(TvaType.Reduced, 100, 105.5)]
    [InlineData(TvaType.Intermediate, 100, 110)]
    public void CalculateTtcFromHt_ShouldCalculateCorrectly(TvaType tvaType, decimal ht, decimal expectedTtc)
    {
        // Act
        var ttc = tvaType.CalculateTtcFromHt(ht);

        // Assert
        ttc.Should().Be(expectedTtc);
    }

    [Theory]
    [InlineData(TvaType.Standard, 120, 100)]
    [InlineData(TvaType.Reduced, 105.5, 100)]
    [InlineData(TvaType.Intermediate, 110, 100)]
    public void CalculateHtFromTtc_ShouldCalculateCorrectly(TvaType tvaType, decimal ttc, decimal expectedHt)
    {
        // Act
        var ht = tvaType.CalculateHtFromTtc(ttc);

        // Assert
        ht.Should().BeApproximately(expectedHt, 0.01m);
    }

    [Fact]
    public void GetRate_WithInvalidEnumValue_ShouldThrowException()
    {
        // Arrange
        var invalidTvaType = (TvaType)999;

        // Act
        Action act = () => invalidTvaType.GetRate();

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
