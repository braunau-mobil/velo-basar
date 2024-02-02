namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IDocumentService
{
    Task<byte[]> CreateLabelAsync(ProductEntity product);

    Task<byte[]> CreateLabelsAsync(IEnumerable<ProductEntity> products);
    
    Task<byte[]> CreateTransactionDocumentAsync(TransactionEntity transaction);
}
