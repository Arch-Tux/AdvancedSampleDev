using AdvancedSampleDev.Application.Suppliers.Dto;
using AdvancedSampleDev.domain.Entities;

namespace AdvancedSampleDev.Application.Suppliers.Mappings;

/// <summary>
/// Extensions de mapping entre les entités du domaine et les DTOs
/// </summary>
public static class SupplierMappingExtensions
{
    /// <summary>
    /// Convertit une entité Supplier du domaine en SupplierResponseDto
    /// </summary>
    public static SupplierResponseDto ToResponseDto(this Supplier supplier)
    {
        return new SupplierResponseDto
        {
            Id = supplier.Id,
            Name = supplier.Name
        };
    }
}
