namespace AdvancedSampleDev.domain.Entities;

using Exceptions;

public class Product
{
  public Guid Id { get; private set; }
  public Price Price { get; private set; }
  private bool IsActive { get; set; }
  
  // Navigation property - gérée par la couche Application/Infrastructure
  public ICollection<Supplier> Suppliers { get; private set; } = new List<Supplier>();

  public Product(Price price)
  {
    Id = Guid.NewGuid();
    Price = price ?? throw new DomainException("Le prix ne peut pas être null.");
    IsActive = true;
  }

  // Méthode publique pour accès (nécessaire pour la persistence)
  public bool GetIsActive() => IsActive;

  public void ChangePrice(Price newPrice)
  {
    if (!IsActive)
    {
      throw new DomainException("Produit inactif");
    }
    
    Price = newPrice ?? throw new DomainException("Le prix ne peut pas être null.");
  }
}