using AdvancedSampleDev.Api.Features.Product.Dto;
using AdvancedSampleDev.domain.Entities;

namespace AdvancedSampleDev.Api.Features.Product.Mappings;

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
    /// Convertit un TvaType (string) en instance Tva du domaine
    /// </summary>
    public static Tva ToTva(this string tvaType)
    {
        return tvaType switch
        {
            "Reduced" => Tva.Reduced,
            "Intermediate" => Tva.Intermediate,
            "Standard" => Tva.Standard,
            _ => Tva.Standard // Par défaut
        };
    }
}
