﻿using BraunauMobil.VeloBasar.Configuration;
using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public record TestContext(IServiceProvider ServiceProvider, HttpClient HttpClient, PrintSettings PrintSettings)
{
    public AcceptanceDocumentModel AcceptanceDocument(string title, string locationAndDateText, string addressText, string sellerIdText, string statusLink, string sellerToken, string signatureText, string productTableCountText, string productTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel> productTableRows)
        => new(title,
                locationAndDateText,
                "Page {0} of {1}",
                "  - powered by https://github.com/braunau-mobil/velo-basar",
                PrintSettings.PageMargins,
                PrintSettings.Acceptance.SubTitle,
                addressText,
                sellerIdText,
                true,
                statusLink,
                PrintSettings.Acceptance.TokenTitle,
                sellerToken,
                "For XYZ: ______________________________",
                signatureText,
                PrintSettings.QrCodeLengthMillimeters,
                new ProductsTableDocumentModel(
                    "Id",
                    "Procuct description",
                    "Size",
                    "Price",
                    "Sum:",
                    productTableCountText,
                    productTablePriceText,
                    null,
                    productTableRows
                )
            );

    public string BankingQrCode(string seller, string amount, string decscription)
    {
        ArgumentNullException.ThrowIfNull(seller);
        ArgumentNullException.ThrowIfNull(amount);
        ArgumentNullException.ThrowIfNull(decscription);

        return "BCD"
            .Line("002")
            .Line("1")
            .Line("SCT")
            .Line()
            .Line(seller)
            .Line()
            .Line(amount)
            .Line()
            .Line()
            .Line(decscription)
            .Line()
            .Line();
    }

    public SaleDocumentModel SaleDocument(string title, string locationAndDateText, string productTableCountText, string productTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel> productTableRows)
        => new(title,
                locationAndDateText,
                "Page {0} of {1}",
                "  - powered by https://github.com/braunau-mobil/velo-basar",
                PrintSettings.PageMargins,
                PrintSettings.Sale.SubTitle,
                true,
                PrintSettings.BannerFilePath!,
                PrintSettings.BannerSubtitle,
                PrintSettings.Website,
                PrintSettings.Sale.HintText,
                PrintSettings.Sale.FooterText,
                new ProductsTableDocumentModel(
                    "Id",
                    "Procuct description",
                    "Size",
                    "Price",
                    "Sum:",
                    productTableCountText,
                    productTablePriceText,
                    PrintSettings.Sale.SellerInfoText,
                    productTableRows
                )
            );

    public SettlementDocumentModel SettlementDocument(string title, string locationAndDateText, string addressText, string sellerIdText, string commissionPartText, string payoutAmountInclComissionText, string payoutCommissionAmountText, string payoutAmountText, string payoutTableCountText, string payoutTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel> payoutTableRows, string pickupTableCountText, string pickupTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel> pickupTableRows, bool addBankingQrCode, string bankAccountHolder, string iban, string bankingQrCodeContent, string signatureText)
        => new(title,
                locationAndDateText,
                "Page {0} of {1}",
                "  - powered by https://github.com/braunau-mobil/velo-basar",
                PrintSettings.PageMargins,
                true,
                PrintSettings.BannerFilePath!,
                PrintSettings.BannerSubtitle,
                PrintSettings.Website,
                addressText,
                sellerIdText,
                new SettlementCommisionSummaryModel(
                   "Revenue from items sold:",
                   "Costs:",
                   "Total amount:",
                   commissionPartText,
                   payoutAmountInclComissionText,
                   payoutCommissionAmountText,
                   payoutAmountText
                ),
                new ProductsTableDocumentModel(
                    "Id",
                    "Procuct description",
                    "Size",
                    "Selling price",
                    "Sum:",
                    payoutTableCountText,
                    payoutTablePriceText,
                    null,
                    payoutTableRows
                ),
                PrintSettings.Settlement.SoldTitle,
                new ProductsTableDocumentModel(
                    "Id",
                    "Procuct description",
                    "Size",
                    "Price",
                    "Sum:",
                    pickupTableCountText,
                    pickupTablePriceText,
                    null,
                    pickupTableRows
                ),
                PrintSettings.Settlement.NotSoldTitle,
                PrintSettings.Settlement.ConfirmationText,
                addBankingQrCode,
                PrintSettings.QrCodeLengthMillimeters,
                bankAccountHolder,
                iban,
                bankingQrCodeContent,
                "Signature ______________________________",
                signatureText
            );
}
