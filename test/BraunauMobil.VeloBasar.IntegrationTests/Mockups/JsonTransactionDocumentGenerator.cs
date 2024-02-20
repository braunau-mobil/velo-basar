using BraunauMobil.VeloBasar.Models.Documents;
using BraunauMobil.VeloBasar.Pdf;

namespace BraunauMobil.VeloBasar.IntegrationTests.Mockups;

public class JsonTransactionDocumentGenerator
    : ITransactionDocumentGenerator
{
    public async Task<byte[]> CreateAcceptanceAsync(AcceptanceDocumentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return await Task.FromResult(model.SerializeAsJson());
    }

    public async Task<byte[]> CreateSaleAsync(SaleDocumentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return await Task.FromResult(model.SerializeAsJson());
    }

    public async Task<byte[]> CreateSettlementAsync(SettlementDocumentModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return await Task.FromResult(model.SerializeAsJson());
    }
}
