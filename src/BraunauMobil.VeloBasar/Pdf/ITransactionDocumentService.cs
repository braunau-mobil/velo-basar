namespace BraunauMobil.VeloBasar.Pdf;

public interface ITransactionDocumentService
{
    Task<byte[]> CreateAsync(TransactionEntity transaction);
}
