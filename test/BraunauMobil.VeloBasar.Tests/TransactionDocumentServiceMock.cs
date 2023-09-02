using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.Tests;

public class TransactionDocumentServiceMock
    : ITransactionDocumentService
{
    public async Task<byte[]> CreateAsync(TransactionEntity transaction)
    {
        //  @todo Add better implementation
        byte[] data = Array.Empty<byte>();
        return await Task.FromResult(data);
    }
}
