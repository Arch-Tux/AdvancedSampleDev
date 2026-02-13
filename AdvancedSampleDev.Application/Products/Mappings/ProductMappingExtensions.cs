using AdvancedSampleDev.Application.Products.Dto;
using AdvancedSampleDev.domain.Entities;

namespace AdvancedSampleDev.Application.Products.Mappings;

/// <summary>
/// Extensions de mapping entre les entités du domaine et les DTOs
/// </summary>
public static class ProductMappingExtensions
{
    /// <summary>
    /// Convertit une entité Product du domaine en ProductResponseDto
    /// </summary>
    public static ProductResponseDto ToResponseDto(this domain.Entities.Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            PriceHt = product.Price.GetAmountHt(),
            PriceTtc = product.Price.GetAmountTtc(),
            TvaRate = product.Price.GetTvaRate(),
            IsActive = product.GetIsActive(),
            Suppliers = product.Suppliers.Select(s => new SupplierDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList()
        };
    }

    /// <summary>
    /// Convertit un TvaType (string) en enum TvaType du domaine
    /// </summary>
    public static TvaType ToTvaType(this string tvaTypeString)
    {
        return tvaTypeString switch
        {
            "Reduced" => TvaType.Reduced,
            "Intermediate" => TvaType.Intermediate,
            "Standard" => TvaType.Standard,
            _ => TvaType.Standard // Par défaut
        };
    }
}
