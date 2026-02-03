namespace AdvancedSampleDev.domain.Entities;

using Exceptions;

public class Supplier
{
  public Guid Id { get; }
  public string Name { get; }

  public Supplier(string name)
  {
    Id = Guid.NewGuid();
    Name = !string.IsNullOrWhiteSpace(name) 
      ? name 
      : throw new DomainException("Le nom du fournisseur ne peut pas être vide.");
  }
}