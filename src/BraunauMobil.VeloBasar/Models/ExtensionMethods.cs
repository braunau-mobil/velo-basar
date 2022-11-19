namespace BraunauMobil.VeloBasar.Models;

public static class ExtensionMethods
{
    public static IReadOnlyList<ProductEntity> GetPayoutProducts(this IEnumerable<ProductToTransactionEntity> productToTransactions)
    {
        ArgumentNullException.ThrowIfNull(productToTransactions);

        return productToTransactions.Select(pt => pt.Product).Where(p => p.ShouldBePayedOut()).ToArray();
    }

    public static IEnumerable<ProductEntity> GetProducts(this IEnumerable<ProductToTransactionEntity> productToTransactions)
    {
        ArgumentNullException.ThrowIfNull(productToTransactions);

        return productToTransactions.Select(pt => pt.Product);
    }

    public static IEnumerable<ProductEntity> GetProductsToPayout(this IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Where(p => p.StorageState == StorageState.Sold || p.StorageState == StorageState.Lost);
    }

    public static IEnumerable<ProductEntity> GetProductsToPickup(this IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Where(p => p.StorageState == StorageState.Available || p.StorageState == StorageState.Locked);
    }

    public static IReadOnlyList<ProductEntity> GetSoldProducts(this IEnumerable<ProductToTransactionEntity> productToTransactions)
    {
        ArgumentNullException.ThrowIfNull(productToTransactions);

        return productToTransactions.Select(pt => pt.Product).Where(p => p.StorageState == StorageState.Sold).ToArray();
    }

    public static decimal SumPrice(this IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Sum(p => p.Price);
    }
}
