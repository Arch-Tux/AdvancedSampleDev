namespace AdvancedSampleDev.Application.Products.Dto;

/// <summary>
/// DTO pour la création d'un produit
/// </summary>
public class CreateProductDto
{
    public required string Name { get; set; }
    public required decimal PriceHt { get; set; }
    public string TvaType { get; set; } = "Standard"; // "Reduced" (5.5%), "Intermediate" (10%), "Standard" (20%)
}
