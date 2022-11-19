using System.ComponentModel.DataAnnotations.Schema;

namespace BraunauMobil.VeloBasar.Models.Entities;

#nullable disable warnings
public sealed class TransactionEntity
    : AbstractEntity
{
    public TransactionEntity()
    { }

    public int BasarId { get; set; }

    public BasarEntity Basar { get; set; }

    public int Number { get; set; }

    public int? DocumentId { get; set; }

    public DateTime TimeStamp { get; set; }

    public TransactionType Type { get; set; }

    public string? Notes { get; set; }

    public int? SellerId { get; set; }

    public SellerEntity? Seller { get; set; }

    public ICollection<ProductToTransactionEntity> Products { get; } = new List<ProductToTransactionEntity>();

    public int? ParentTransactionId { get; set; }

    public TransactionEntity? ParentTransaction { get; set; }

    public ICollection<TransactionEntity> ChildTransactions { get; } = new List<TransactionEntity>();

    [NotMapped]
    public ChangeInfo Change { get; set; }

    public bool CanCancel
    {
        get => Type == TransactionType.Sale;
    }

    public bool CanHasDocument
    {
        get => Type == TransactionType.Acceptance
            || Type == TransactionType.Sale
            || Type == TransactionType.Settlement;
    }

    public bool HasDocument
    {
        get => DocumentId.HasValue
            && Type != TransactionType.Acceptance;
    }

    public bool NeedsStatusPush
    {
        get => Type == TransactionType.Acceptance
            || Type == TransactionType.Cancellation
            || Type == TransactionType.Sale
            || Type == TransactionType.Settlement;
    }

    public decimal GetPayoutTotal()
        => Products.GetPayoutProducts().SumPrice();

    public decimal GetPayoutCommissionTotal()
        => Products.GetPayoutProducts().Sum(p => p.GetCommissionAmount(Basar));

    public decimal GetPayoutTotalWithoutCommission()
        => GetPayoutTotal() - GetPayoutCommissionTotal();

    public decimal GetProductsSum()
        => Products.Select(pt => pt.Product).SumPrice();

    public decimal GetSoldProductsSum()
        => Products.GetSoldProducts().SumPrice();

    public decimal GetSoldTotal()
        => Products.GetSoldProducts().Sum(p => p.GetCommissionedPrice(Basar));
}
