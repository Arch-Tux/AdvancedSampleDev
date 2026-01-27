namespace AdvancedSampleDev.domain.Entities;

using Exceptions;

public class Price
{
  public decimal Amount { get; }

  public Price(decimal amount)
  {
    if (amount <= 0)
    {
      throw new DomainException("Le prix doit être supérieur à zéro.");
    }
    
    Amount = amount;
  }
  
  // Equality pour Value Object
  public override bool Equals(object? obj)
  {
    if (obj is not Price other) return false;
    return Amount == other.Amount;
  }

  public override int GetHashCode() => Amount.GetHashCode();

  public override string ToString() => $"{Amount:C}";
}