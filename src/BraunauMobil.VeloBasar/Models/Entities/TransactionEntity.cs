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
        get => Type == TransactionType.Sale
            && Products.GetProducts().Any(product => product.IsAllowed(TransactionType.Cancellation));
    }

    public bool CanHasDocument
    {
        get => Type == TransactionType.Acceptance
            || Type == TransactionType.Sale
            || Type == TransactionType.Settlement;
    }

    public bool NeedsStatusPush
    {
        get => Type == TransactionType.Acceptance
            || Type == TransactionType.Cancellation
            || Type == TransactionType.Sale
            || Type == TransactionType.Settlement;
    }

    public bool NeedsBankingQrCodeOnDocument
    {
        get
        {
            return Type == TransactionType.Settlement
                && !string.IsNullOrEmpty(Seller.IBAN)
                && Products.Any(pt => pt.Product.StorageState == StorageState.Sold);
        }
    }

    public bool UpdateDocumentOnDemand
    {
        get => Type == TransactionType.Acceptance;
    }

    public decimal GetPayoutAmountInclCommission()
        => Products.GetPayoutProducts().SumPrice();

    public decimal GetPayoutCommissionAmount()
        => Products.GetPayoutProducts().Sum(p => p.GetCommissionAmount(Basar));

    public decimal GetPayoutAmount()
        => GetPayoutAmountInclCommission() - GetPayoutCommissionAmount();

    public decimal GetSoldProductsSum()
        => Products.GetSoldProducts().SumPrice();

    public decimal GetProductsValue()
        => Products.GetProducts().SumPrice();
}
