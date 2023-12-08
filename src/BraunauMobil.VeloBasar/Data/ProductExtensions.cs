using Microsoft.EntityFrameworkCore;

namespace BraunauMobil.VeloBasar.Data;

public static class ProductExtensions
{
    public static IQueryable<string> Brands(this IQueryable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Select(p => p.Brand)
            .Distinct()
            .OrderBy(_ => _);
    }

    public static async Task<int> GetBasarIdAsync(this IQueryable<ProductEntity> products, int productId)
    {
        ArgumentNullException.ThrowIfNull(products);

        return await products
            .Where(product => product.Id == productId)
            .Select(product => product.Session.BasarId)
            .SingleAsync();
    }    

    public static async Task<IReadOnlyList<ProductEntity>> GetForBasarAsync(this IQueryable<ProductEntity> products, int basarId)
    {
        ArgumentNullException.ThrowIfNull(products);

        return await products
            .Include(p => p.Type)
            .Where(p => p.Session.BasarId == basarId)
            .ToArrayAsync();
    }

    public static async Task<IReadOnlyList<ProductEntity>> GetForSellerAsync(this IQueryable<ProductEntity> products, int basarId, int sellerId)
    {
        ArgumentNullException.ThrowIfNull(products);

        return await products
            .Include(p => p.Type)
            .Where(p => p.Session.BasarId == basarId && p.Session.SellerId == sellerId)
            .ToArrayAsync();
    }

    public static async Task<IReadOnlyList<ProductEntity>> GetManyAsync(this IQueryable<ProductEntity> products, IEnumerable<int> ids)
    {
        ArgumentNullException.ThrowIfNull(products);
        ArgumentNullException.ThrowIfNull(ids);

        return await products
            .Where(product => ids.Contains(product.Id))
            .ToArrayAsync();
    }

    public static IQueryable<ProductEntity> IncludeAll(this IQueryable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products
            .Include(product => product.Type)
            .Include(product => product.Session)
                .ThenInclude(session => session.Basar)
            .Include(product => product.Session)
                .ThenInclude(session => session.Seller)
                    .ThenInclude(seller => seller.Country)
        ;
    }

    public static IQueryable<ProductEntity> WhereBasarAndSeller(this IQueryable<ProductEntity> products, int basarId, int sellerId)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products
            .Where(p => p.Session.BasarId == basarId && p.Session.SellerId == sellerId);
    }
}
