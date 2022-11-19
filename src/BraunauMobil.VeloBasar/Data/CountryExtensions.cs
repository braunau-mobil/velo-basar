namespace BraunauMobil.VeloBasar.Data;

public static class CountryExtensions
{
    public static IQueryable<CountryEntity> DefaultOrder(this IQueryable<CountryEntity> countries)
    {
        ArgumentNullException.ThrowIfNull(countries);

        return countries.OrderBy(c => c.Iso3166Alpha3Code);
    }
}
