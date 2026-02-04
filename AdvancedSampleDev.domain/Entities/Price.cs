namespace AdvancedSampleDev.domain.Entities;

using Exceptions;

public class Price
{
  private decimal AmountHt { get; }
  private Tva Tva { get; }
  
  private decimal AmountTtc => Tva.CalculateTtcFromHt(AmountHt);

  public Price(decimal amountHt, Tva tva)
  {
    AmountHt = amountHt > 0 
      ? amountHt 
      : throw new DomainException("Le prix HT doit être supérieur à zéro.");
    
    Tva = tva ?? throw new DomainException("La TVA ne peut pas être null.");
  }

  // Méthodes publiques pour accès (nécessaires pour la persistence)
  public decimal GetAmountHt() => AmountHt;
  public decimal GetAmountTtc() => AmountTtc;
  public decimal GetTvaRate() => Tva.Rate;
  
  // Equality pour Value Object
  public override bool Equals(object? obj)
  {
    if (obj is not Price other) return false;
    return AmountHt == other.AmountHt && Tva.Rate == other.Tva.Rate;
  }

  public override int GetHashCode() => HashCode.Combine(AmountHt, Tva.Rate);

  public override string ToString() => $"HT: {AmountHt:C} | TTC: {AmountTtc:C}";
}