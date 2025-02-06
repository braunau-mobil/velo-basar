using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public class DocumentService(IDocumentModelFactory factory, IProductLabelGenerator productLabelGenerator, ITransactionDocumentGenerator transactionDocumentGenerator)
    : IDocumentService
{
    public async Task<byte[]> CreateLabelAsync(ProductEntity product)
    {
        ArgumentNullException.ThrowIfNull(product);

        return await productLabelGenerator.CreateLabelAsync(factory.CreateProductLabelModel(product), factory.LabelPrintSettings);
    }

    public async Task<byte[]> CreateLabelsAsync(IEnumerable<ProductEntity> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return await productLabelGenerator.CreateLabelsAsync(products.Select(factory.CreateProductLabelModel), factory.LabelPrintSettings);
    }

    public async Task<byte[]> CreateTransactionDocumentAsync(TransactionEntity transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (transaction.Type == TransactionType.Acceptance)
        {
            return await transactionDocumentGenerator.CreateAcceptanceAsync(factory.CreateAcceptanceModel(transaction));
        }
        else if (transaction.Type == TransactionType.Sale)
        {
            return await transactionDocumentGenerator.CreateSaleAsync(factory.CreateSaleModel(transaction));
        }
        else if (transaction.Type == TransactionType.Settlement)
        {
            return await transactionDocumentGenerator.CreateSettlementAsync(factory.CreateSettlementModel(transaction));
        }

        throw new InvalidOperationException($"Cannot generate transaction document for: {transaction.Type}");
    }
}
