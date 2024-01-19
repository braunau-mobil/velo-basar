namespace BraunauMobil.VeloBasar.Models;

public sealed class SellerDetailsModel
{
    public SellerDetailsModel(SellerEntity entity)
    {
        Entity = entity ?? throw new ArgumentNullException(nameof(entity));
    }

    public SellerEntity Entity { get; init; }

    public int BasarId { get; init; }

    public bool CanPushStatus { get; init; }

    public int AcceptedProductCount { get; init; }

    public int SoldProductCount { get; init; }

    public int NotSoldProductCount { get; init; }

    public int PickedUpProductCount { get; init; }

    public decimal SettlementAmout { get; init; }

    public IReadOnlyList<TransactionEntity> Transactions { get; init; } = Array.Empty<TransactionEntity>();

    public IReadOnlyList<ProductEntity> Procucts { get; init; } = Array.Empty<ProductEntity>();
}
