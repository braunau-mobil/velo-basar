using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IDocumentModelFactory
{
    LabelPrintSettings LabelPrintSettings { get; }
    
    ProductLabelDocumentModel CreateProductLabelModel(ProductEntity product);

    AcceptanceDocumentModel CreateAcceptanceModel(TransactionEntity acceptance);

    SaleDocumentModel CreateSaleModel(TransactionEntity sale);

    SettlementDocumentModel CreateSettlementModel(TransactionEntity settlement);
}
