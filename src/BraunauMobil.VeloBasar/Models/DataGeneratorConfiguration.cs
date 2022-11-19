namespace BraunauMobil.VeloBasar.Models;

#nullable disable warnings
public sealed class DataGeneratorConfiguration
    : InitializationConfiguration
{
    public DataGeneratorConfiguration()
    {
        FirstBasarDate = new DateTime(2063, 4, 5);
        BasarCount = 1;
        MinAcceptancesPerSeller = 1;
        MaxAcceptancesPerSeller = 3;
        MinSellers = 10;
        MaxSellers = 30;
        MeanPrice = 100.0m;
        StdDevPrice = 15.0m;
        MeanProductsPerSeller = 1.3;
        StdDevProductsPerSeller = 4.5;
    }

    public DateTime FirstBasarDate { get; set; }

    public int BasarCount { get; set; }

    public int MinAcceptancesPerSeller { get; set; }

    public int MaxAcceptancesPerSeller { get; set; }

    public int MinSellers { get; set; }

    public int MaxSellers { get; set; }

    public double MeanProductsPerSeller { get; set; }

    public double StdDevProductsPerSeller { get; set; }

    public decimal MeanPrice { get; set; }

    public decimal StdDevPrice { get; set; }

    public bool SimulateSales { get; set; }
}
