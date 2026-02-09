namespace AdvancedSampleDev.Application.Suppliers.Dto;

/// <summary>
/// DTO pour la réponse contenant les informations d'un fournisseur
/// </summary>
public class SupplierResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
