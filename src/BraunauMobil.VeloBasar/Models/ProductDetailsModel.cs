namespace BraunauMobil.VeloBasar.Models;

public sealed class ProductDetailsModel
{
    public ProductDetailsModel(ProductEntity entity)
    {
        Entity= entity ?? throw new ArgumentNullException(nameof(entity));
    }

    public bool CanLock { get; init; }

    public bool CanSetAsLost { get; init; }

    public bool CanUnlock { get; init; }

    public ProductEntity Entity { get; }

    public IReadOnlyList<TransactionEntity> Transactions { get; init; } = Array.Empty<TransactionEntity>();
}
