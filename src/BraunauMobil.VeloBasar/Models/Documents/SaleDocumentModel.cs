using BraunauMobil.VeloBasar.Configuration;

namespace BraunauMobil.VeloBasar.Models.Documents;

public record SaleDocumentModel(
    string Title,
    string LocationAndDateText,
    string PageNumberFormat,
    string PoweredBy,
    Margins PageMargins,
    string Subtitle,
    bool addBanner,
    string BannerFilePath,
    string BannerSubtitle,
    string Website,
    string HintText,
    string FooterText,
    ProductsTableDocumentModel ProductsTable
)
    : ITransactionDocumentModel
{ }
