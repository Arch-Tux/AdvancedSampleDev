using AdvancedSampleDev.Application.Suppliers;
using AdvancedSampleDev.Application.Suppliers.Dto;
using AdvancedSampleDev.Application.Suppliers.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedSampleDev.Api.Features.Supplier;

/// <summary>
/// Contrôleur pour gérer les fournisseurs
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly SupplierService _supplierService;
    private readonly ILogger<SuppliersController> _logger;

    public SuppliersController(SupplierService supplierService, ILogger<SuppliersController> logger)
    {
        _supplierService = supplierService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère tous les fournisseurs
    /// </summary>
    /// <returns>Liste des fournisseurs</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SupplierResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SupplierResponseDto>>> GetAll()
    {
        var suppliers = await _supplierService.GetAllAsync();
        var response = suppliers.Select(s => s.ToResponseDto());
        return Ok(response);
    }

    /// <summary>
    /// Récupère un fournisseur par son ID
    /// </summary>
    /// <param name="id">ID du fournisseur</param>
    /// <returns>Le fournisseur correspondant</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SupplierResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SupplierResponseDto>> GetById(Guid id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        
        if (supplier == null)
        {
            return NotFound(new { message = $"Fournisseur avec l'ID {id} introuvable" });
        }

        return Ok(supplier.ToResponseDto());
    }

    /// <summary>
    /// Crée un nouveau fournisseur
    /// </summary>
    /// <param name="dto">Données du fournisseur à créer</param>
    /// <returns>Le fournisseur créé</returns>
    [HttpPost]
    [ProducesResponseType(typeof(SupplierResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SupplierResponseDto>> Create([FromBody] CreateSupplierDto dto)
    {
        try
        {
            var supplier = await _supplierService.CreateAsync(dto.Name);
            var response = supplier.ToResponseDto();
            
            return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création du fournisseur");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Met à jour un fournisseur existant
    /// </summary>
    /// <param name="id">ID du fournisseur</param>
    /// <param name="dto">Nouvelles données du fournisseur</param>
    /// <returns>Le fournisseur mis à jour</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SupplierResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SupplierResponseDto>> Update(Guid id, [FromBody] UpdateSupplierDto dto)
    {
        try
        {
            var supplier = await _supplierService.UpdateAsync(id, dto.Name);
            
            if (supplier == null)
            {
                return NotFound(new { message = $"Fournisseur avec l'ID {id} introuvable" });
            }

            return Ok(supplier.ToResponseDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour du fournisseur {SupplierId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Supprime un fournisseur
    /// </summary>
    /// <param name="id">ID du fournisseur</param>
    /// <returns>Confirmation de suppression</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _supplierService.DeleteAsync(id);
        
        if (!deleted)
        {
            return NotFound(new { message = $"Fournisseur avec l'ID {id} introuvable" });
        }

        return NoContent();
    }
}
