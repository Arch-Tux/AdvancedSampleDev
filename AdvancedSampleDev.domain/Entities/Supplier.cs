namespace AdvancedSampleDev.domain.Entities;

using Exceptions;

public class Supplier
{
  public Guid Id { get; private set; }
  public string Name { get; private set; }

  // Constructeur public pour créer un nouveau fournisseur
  public Supplier(string name)
  {
    Id = Guid.NewGuid();
    Name = ValidateName(name);
  }

  // Constructeur privé pour reconstituer depuis la persistance
  private Supplier(Guid id, string name)
  {
    Id = id;
    Name = ValidateName(name);
  }

  // Factory method pour Infrastructure - reconstitution depuis la base de données
  public static Supplier Reconstitute(Guid id, string name)
  {
    return new Supplier(id, name);
  }

  // Méthode pour changer le nom
  public void ChangeName(string newName)
  {
    Name = ValidateName(newName);
  }

  // Validation centralisée du nom
  private static string ValidateName(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new DomainException("Le nom du fournisseur ne peut pas être vide.");
    }
    return name;
  }
}

