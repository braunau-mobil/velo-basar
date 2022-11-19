namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class ProductToTransactionEntity
{
    public ProductToTransactionEntity()
    { }

    public ProductToTransactionEntity(TransactionEntity transaction, ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        ArgumentNullException.ThrowIfNull(product);

        Transaction = transaction;
        TransactionId = transaction.Id;
        Product = product;
        ProductId = product.Id;

        Product.SetState(Transaction.Type);
    }

    public int ProductId { get; set; }

    public ProductEntity Product { get; set; }

    public int TransactionId { get; set; }

    public TransactionEntity Transaction { get; set; }
}
