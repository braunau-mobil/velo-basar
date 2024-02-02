using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public interface IDocumentModelFactory
{
    ProductLabelDocumentModel CreateProductLabelModel(ProductEntity product);

    AcceptanceDocumentModel CreateAcceptanceModel(TransactionEntity acceptance);

    SaleDocumentModel CreateSaleModel(TransactionEntity sale);

    SettlementDocumentModel CreateSettlementModel(TransactionEntity settlement);
}
