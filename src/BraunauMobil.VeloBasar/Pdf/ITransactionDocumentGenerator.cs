using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Pdf;

public interface ITransactionDocumentGenerator
{
    Task<byte[]> CreateAcceptanceAsync(AcceptanceDocumentModel model);

    Task<byte[]> CreateSaleAsync(SaleDocumentModel model);

    Task<byte[]> CreateSettlementAsync(SettlementDocumentModel model);
}
