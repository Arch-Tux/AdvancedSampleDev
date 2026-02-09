namespace AdvancedSampleDev.domain.Entities;

using Exceptions;

public class Price
{
  private decimal AmountHt { get; }
  private TvaType TvaType { get; }
  
  private decimal AmountTtc => TvaType.CalculateTtcFromHt(AmountHt);

  public Price(decimal amountHt, TvaType tvaType)
  {
    AmountHt = amountHt > 0 
      ? amountHt 
      : throw new DomainException("Le prix HT doit être supérieur à zéro.");
    
    TvaType = tvaType;
  }

  // Méthodes publiques pour accès
  public decimal GetAmountHt() => AmountHt;
  public decimal GetAmountTtc() => AmountTtc;
  public decimal GetTvaRate() => TvaType.GetRate();
  public TvaType GetTvaType() => TvaType;
  
  // Equality pour Value Object
  public override bool Equals(object? obj)
  {
    if (obj is not Price other) return false;
    return AmountHt == other.AmountHt && TvaType == other.TvaType;
  }

  public override int GetHashCode() => HashCode.Combine(AmountHt, TvaType);

  public override string ToString() => $"HT: {AmountHt:C} | TTC: {AmountTtc:C}";
}