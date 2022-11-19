namespace BraunauMobil.VeloBasar.Data;

public static class ProductTypeExtensions
{
    public static IQueryable<ProductTypeEntity> DefaultOrder(this IQueryable<ProductTypeEntity> productTypes)
    {
        ArgumentNullException.ThrowIfNull(productTypes);
        return productTypes.OrderBy(b => b.Name);
    }
}
