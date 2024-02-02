using BraunauMobil.VeloBasar.Configuration;

namespace BraunauMobil.VeloBasar.Models.Documents;

public record AcceptanceDocumentModel(
    string Title,
    string LocationAndDateText,
    string PageNumberFormat,
    string PoweredBy,
    Margins PageMargins,
    string SubTitle,
    string AddressText,
    string SellerIdText,
    bool AddTokenAndStatusLink,
    string StatusLink,
    string TokenTitle,
    string SellerToken,
    string SignatureLine,
    string SignatureText,
    int QrCodeLengthMillimeters,
    ProductsTableDocumentModel ProductsTable
)
    : ITransactionDocumentModel
{ }
