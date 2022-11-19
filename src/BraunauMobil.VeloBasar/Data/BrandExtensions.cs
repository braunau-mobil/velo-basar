namespace BraunauMobil.VeloBasar.Data;

public static class BrandExtensions
{
    public static IQueryable<BrandEntity> DefaultOrder(this IQueryable<BrandEntity> brands)
    {
        ArgumentNullException.ThrowIfNull(brands);
        return brands.OrderBy(b => b.Name);
    }
}
