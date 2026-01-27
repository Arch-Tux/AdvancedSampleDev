namespace AdvancedSampleDev.domain.Entities;

using Exceptions;

public class Product
{
  public Guid Id { get; private  set; } // Identité
  public decimal Price { get; private  set; }
  public bool IsActive { get; private  set; } // Par défaut : false

  public Product()
  {
    IsActive = true;
  }

  public void ChangePrice(decimal newPrice)
  {
    if (newPrice <= 0)
    {
      throw new DomainException("Le prix doit être supérieur à zéro.");
    }

    if (!IsActive)
    {
      throw new DomainException("Produit inactif");
    }
    
    Price = newPrice;
  }
}