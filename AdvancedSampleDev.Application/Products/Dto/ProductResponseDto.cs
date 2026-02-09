namespace AdvancedSampleDev.Application.Products.Dto;

/// <summary>
/// DTO pour la réponse contenant les informations d'un produit
/// </summary>
public class ProductResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required decimal PriceHt { get; set; }
    public required decimal PriceTtc { get; set; }
    public decimal TvaRate { get; set; }
    public required bool IsActive { get; set; }
    public List<SupplierDto> Suppliers { get; set; } = new();
}

/// <summary>
/// DTO pour un fournisseur dans la réponse produit
/// </summary>
public class SupplierDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
