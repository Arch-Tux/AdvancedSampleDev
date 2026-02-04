namespace AdvancedSampleDev.domain.Entities;

using Exceptions;

public class Product
{
  public Guid Id { get; private set; }
  public string Name { get; private set; }
  public Price Price { get; private set; }
  private bool IsActive { get; set; }
  
  // Navigation property - gérée par la couche Application/Infrastructure
  public ICollection<Supplier> Suppliers { get; private set; } = new List<Supplier>();

  // Constructeur public pour créer un nouveau produit
  public Product(string name, Price price)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new DomainException("Le nom du produit ne peut pas être vide.");
    }
    
    Id = Guid.NewGuid();
    Name = name;
    Price = price ?? throw new DomainException("Le prix ne peut pas être null.");
    IsActive = true;
  }

  // Constructeur privé pour reconstituer depuis la persistance (utilisé par Infrastructure)
  private Product(Guid id, string name, Price price, bool isActive)
  {
    Id = id;
    Name = name ?? throw new DomainException("Le nom du produit ne peut pas être null.");
    Price = price ?? throw new DomainException("Le prix ne peut pas être null.");
    IsActive = isActive;
  }

  // Factory method pour Infrastructure - reconstitution depuis la base de données
  public static Product Reconstitute(Guid id, string name, Price price, bool isActive)
  {
    return new Product(id, name, price, isActive);
  }

  // Méthode publique pour accès
  public bool GetIsActive() => IsActive;

  public void Activate()
  {
    IsActive = true;
  }

  public void Deactivate()
  {
    IsActive = false;
  }

  public void ChangeName(string newName)
  {
    if (string.IsNullOrWhiteSpace(newName))
    {
      throw new DomainException("Le nom du produit ne peut pas être vide.");
    }
    
    Name = newName;
  }

  public void ChangePrice(Price newPrice)
  {
    if (!IsActive)
    {
      throw new DomainException("Produit inactif");
    }
    
    Price = newPrice ?? throw new DomainException("Le prix ne peut pas être null.");
  }
}