namespace AdvancedSampleDev.Application.Products.Dto;

/// <summary>
/// DTO pour la mise à jour d'un produit
/// </summary>
public class UpdateProductDto
{
    public required string Name { get; set; }
    public required decimal PriceHt { get; set; }
    public string TvaType { get; set; } = "Standard";
}
