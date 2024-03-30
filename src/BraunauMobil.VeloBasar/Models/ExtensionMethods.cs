namespace BraunauMobil.VeloBasar.Models;

public static class ExtensionMethods
{
    public static IEnumerable<ProductEntity> GetPaybackProducts(this IEnumerable<ProductToTransactionEntity> productToTransactions)
    {
        ArgumentNullException.ThrowIfNull(productToTransactions);

        return productToTransactions.GetProducts().Where(p => p.ShouldBePayedBack());
    }

    public static IEnumerable<ProductEntity> GetPayoutProducts(this IEnumerable<ProductToTransactionEntity> productToTransactions)
    {
        ArgumentNullException.ThrowIfNull(productToTransactions);

        return productToTransactions.GetProducts().Where(p => p.ShouldBePayedOut());
    }

    public static IEnumerable<ProductEntity> GetProductsToPickup(this IEnumerable<ProductToTransactionEntity> productToTransactions)
    {
        ArgumentNullException.ThrowIfNull(productToTransactions);

        return productToTransactions.GetProducts().Where(p => p.ShouldBePickedUp());
    }

    public static IEnumerable<ProductEntity> GetProducts(this IEnumerable<ProductToTransactionEntity> productToTransactions)
    {
        ArgumentNullException.ThrowIfNull(productToTransactions);

        return productToTransactions.Select(pt => pt.Product);
    }

    public static IEnumerable<ProductEntity> GetSoldProducts(this IEnumerable<ProductToTransactionEntity> productToTransactions)
    {
        ArgumentNullException.ThrowIfNull(productToTransactions);

        return productToTransactions.Select(pt => pt.Product).Where(p => p.StorageState == StorageState.Sold);
    }

    public static decimal SumPrice(this IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Sum(p => p.Price);
    }
}
