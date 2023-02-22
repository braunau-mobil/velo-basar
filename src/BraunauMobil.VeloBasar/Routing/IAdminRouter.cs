namespace BraunauMobil.VeloBasar.Routing;

public interface IAdminRouter
{
    string ToCreateSampleAcceptanceDocument();

    string ToCreateSampleLabels();

    string ToCreateSampleSaleDocument();

    string ToCreateSampleSettlementDocument();

    string ToExport();

    string ToExportSellersForNewsletter();

    string ToPrintTest();
}
