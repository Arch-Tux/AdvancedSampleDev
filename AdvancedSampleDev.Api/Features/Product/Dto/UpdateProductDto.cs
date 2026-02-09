namespace AdvancedSampleDev.Api.Features.Product.Dto;

/// <summary>
/// DTO pour la mise à jour d'un produit
/// </summary>
public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal PriceHt { get; set; }
    public string TvaType { get; set; } = "Standard";
}
