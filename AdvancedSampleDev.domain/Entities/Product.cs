namespace AdvancedSampleDev.domain.Entities;

using Exceptions;

public class Product
{
  public Guid Id { get; private set; } // Identité
  public Price Price { get; private set; }
  private bool IsActive { get; set; } // Par défaut : false

  public Product(Price price)
  {
    Price = price ?? throw new DomainException("Le prix ne peut pas être null.");
    IsActive = true;
  }

  public void ChangePrice(Price newPrice)
  {
    if (newPrice == null)
    {
      throw new DomainException("Le prix ne peut pas être null.");
    }

    if (!IsActive)
    {
      throw new DomainException("Produit inactif");
    }
    
    Price = newPrice;
  }
}