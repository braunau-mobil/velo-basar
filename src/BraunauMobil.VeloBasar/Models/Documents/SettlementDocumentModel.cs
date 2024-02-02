using BraunauMobil.VeloBasar.Configuration;

namespace BraunauMobil.VeloBasar.Models.Documents;

public record SettlementDocumentModel
    (
    string Title,
    string LocationAndDateText,
    string PageNumberFormat,
    string PoweredBy,
    Margins PageMargins,
    bool AddBanner,
    string BannerFilePath,
    string BannerSubtitle,
    string Website,
    string AddressText,
    string SellerIdText,
    SettlementCommisionSummaryModel? CommissionSummary,
    ProductsTableDocumentModel? PayoutProductsTable,
    string PayoutProductsTableTitle,
    ProductsTableDocumentModel? PickupProductsTable,
    string PickupProductsTableTitle,
    string ConfirmationText,
    bool AddBankingQrCode,
    int QrCodeLengthMillimeters,
    string BankAccountHolder,
    string IBAN,
    string BankingQrCodeContent,
    string SignatureLine,
    string SignatureText
)
    : ITransactionDocumentModel
{ }


public record SettlementCommisionSummaryModel(
    string IncomeFromSoldProductsText,
    string CostsText,
    string TotalAmountText,
    string CommissionPartText,
    string PayoutAmountInclCommissionText,
    string PayoutCommissionAmountText,
    string PpayoutAmountText);
