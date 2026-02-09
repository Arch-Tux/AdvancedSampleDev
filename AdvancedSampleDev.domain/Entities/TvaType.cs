namespace AdvancedSampleDev.domain.Entities;

/// <summary>
/// Énumération des différents taux de TVA applicables en France
/// </summary>
public enum TvaType
{
    /// <summary>
    /// TVA réduite - 5.5%
    /// </summary>
    Reduced = 0,
    
    /// <summary>
    /// TVA intermédiaire - 10%
    /// </summary>
    Intermediate = 1,
    
    /// <summary>
    /// TVA normale/standard - 20%
    /// </summary>
    Standard = 2
}

/// <summary>
/// Extensions pour l'enum TvaType
/// </summary>
public static class TvaTypeExtensions
{
    /// <summary>
    /// Obtient le taux de TVA décimal pour un type de TVA donné
    /// </summary>
    public static decimal GetRate(this TvaType tvaType)
    {
        return tvaType switch
        {
            TvaType.Reduced => 0.055m,      // 5.5%
            TvaType.Intermediate => 0.10m,   // 10%
            TvaType.Standard => 0.20m,       // 20%
            _ => throw new ArgumentOutOfRangeException(nameof(tvaType), tvaType, "Type de TVA non reconnu")
        };
    }
    
    /// <summary>
    /// Calcule le montant TTC à partir du HT
    /// </summary>
    public static decimal CalculateTtcFromHt(this TvaType tvaType, decimal ht)
    {
        return ht * (1 + tvaType.GetRate());
    }
    
    /// <summary>
    /// Calcule le montant HT à partir du TTC
    /// </summary>
    public static decimal CalculateHtFromTtc(this TvaType tvaType, decimal ttc)
    {
        return ttc / (1 + tvaType.GetRate());
    }
}
