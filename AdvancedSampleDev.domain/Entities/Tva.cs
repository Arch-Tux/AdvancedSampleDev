namespace AdvancedSampleDev.domain.Entities;

public class Tva
{
  public decimal Rate { get; }

  private Tva(decimal rate)
  {
    Rate = rate;
  }

  // Singletons - Instances uniques immuables des 3 taux de TVA en France
  private static readonly Tva ReducedInstance = new(0.055m);
  private static readonly Tva IntermediateInstance = new(0.10m);
  private static readonly Tva StandardInstance = new(0.20m);

  public static Tva Reduced => ReducedInstance;           // TVA réduite 5,5%
  public static Tva Intermediate => IntermediateInstance; // TVA intermédiaire 10%
  public static Tva Standard => StandardInstance;         // TVA normale 20%

  public decimal CalculateTtcFromHt(decimal ht) => ht * (1 + Rate);

  public decimal CalculateHtFromTtc(decimal ttc) => ttc / (1 + Rate);
}