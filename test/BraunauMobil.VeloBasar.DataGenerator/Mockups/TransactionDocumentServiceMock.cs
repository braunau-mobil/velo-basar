using BraunauMobil.VeloBasar.Models.Documents;
using BraunauMobil.VeloBasar.Pdf;
using System.Text;

namespace BraunauMobil.VeloBasar.DataGenerator.Mockups;

public class TransactionDocumentServiceMock
    : ITransactionDocumentGenerator
{
    public async Task<byte[]> CreateAcceptanceAsync(AcceptanceDocumentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return await CreateAsync(model);
    }

    public async Task<byte[]> CreateSaleAsync(SaleDocumentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return await CreateAsync(model);
    }

    public async Task<byte[]> CreateSettlementAsync(SettlementDocumentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        return await CreateAsync(model);
    }
    
    private static async Task<byte[]> CreateAsync(ITransactionDocumentModel transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        byte[] bytes = Encoding.UTF8.GetBytes(transaction.Title);

        return await Task.FromResult(bytes);
    }
}
