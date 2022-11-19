namespace BraunauMobil.VeloBasar.Data;

public static class SellerExtensions
{
    public static IQueryable<SellerEntity> DefaultOrder(this IQueryable<SellerEntity> sellers)
    {
        ArgumentNullException.ThrowIfNull(sellers);
        return sellers.OrderBy(seller => seller.FirstName)
            .ThenBy(seller => seller.LastName);
    }
}
