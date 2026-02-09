namespace AdvancedSampleDev.Api.Features.Product.Dto;

/// <summary>
/// DTO pour la création d'un produit
/// </summary>
public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal PriceHt { get; set; }
    public string TvaType { get; set; } = "Standard"; // "Reduced" (5.5%), "Intermediate" (10%), "Standard" (20%)
}
