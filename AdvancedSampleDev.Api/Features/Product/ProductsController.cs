using AdvancedSampleDev.Api.Features.Product.Dto;
using AdvancedSampleDev.Api.Features.Product.Mappings;
using AdvancedSampleDev.Application.Services;
using AdvancedSampleDev.domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedSampleDev.Api.Features.Product;

/// <summary>
/// Contrôleur pour gérer les produits
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère tous les produits
    /// </summary>
    /// <returns>Liste des produits</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        var response = products.Select(p => p.ToResponseDto());
        return Ok(response);
    }

    /// <summary>
    /// Récupère un produit par son ID
    /// </summary>
    /// <param name="id">ID du produit</param>
    /// <returns>Le produit correspondant</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponseDto>> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);
        
        if (product == null)
        {
            return NotFound(new { message = $"Produit avec l'ID {id} introuvable" });
        }

        return Ok(product.ToResponseDto());
    }

    /// <summary>
    /// Crée un nouveau produit
    /// </summary>
    /// <param name="dto">Données du produit à créer</param>
    /// <returns>Le produit créé</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductResponseDto>> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            var tva = dto.TvaType.ToTva();
            var price = new Price(dto.PriceHt, tva);
            
            var product = await _productService.CreateAsync(dto.Name, price);
            var response = product.ToResponseDto();
            
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création du produit");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Met à jour un produit existant
    /// </summary>
    /// <param name="id">ID du produit</param>
    /// <param name="dto">Nouvelles données du produit</param>
    /// <returns>Le produit mis à jour</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductResponseDto>> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var tva = dto.TvaType.ToTva();
            var price = new Price(dto.PriceHt, tva);
            
            var product = await _productService.UpdateAsync(id, dto.Name, price);
            
            if (product == null)
            {
                return NotFound(new { message = $"Produit avec l'ID {id} introuvable" });
            }

            return Ok(product.ToResponseDto());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour du produit {ProductId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Supprime un produit
    /// </summary>
    /// <param name="id">ID du produit</param>
    /// <returns>Confirmation de suppression</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _productService.DeleteAsync(id);
        
        if (!deleted)
        {
            return NotFound(new { message = $"Produit avec l'ID {id} introuvable" });
        }

        return NoContent();
    }

    /// <summary>
    /// Active un produit
    /// </summary>
    /// <param name="id">ID du produit</param>
    /// <returns>Confirmation d'activation</returns>
    [HttpPatch("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(Guid id)
    {
        var activated = await _productService.ActivateAsync(id);
        
        if (!activated)
        {
            return NotFound(new { message = $"Produit avec l'ID {id} introuvable" });
        }

        return Ok(new { message = "Produit activé avec succès" });
    }

    /// <summary>
    /// Désactive un produit
    /// </summary>
    /// <param name="id">ID du produit</param>
    /// <returns>Confirmation de désactivation</returns>
    [HttpPatch("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var deactivated = await _productService.DeactivateAsync(id);
        
        if (!deactivated)
        {
            return NotFound(new { message = $"Produit avec l'ID {id} introuvable" });
        }

        return Ok(new { message = "Produit désactivé avec succès" });
    }
}
